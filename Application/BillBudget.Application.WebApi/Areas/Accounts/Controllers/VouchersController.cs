using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Accounts.Logic.Abstractions;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Helpers;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Domain.Helpers;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.ViewModels;
using BillBudget.Voucher.Data.Vouchers;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BillBudget.Application.WebApi.Areas.Accounts.Controllers
{
    //[Authorize]
    [Route("api/vouchers")]
    public class VouchersController : Controller
    {
        #region Dependencies
        private readonly IVoucherRepository _voucherBuilder;
        private readonly IFinancialTransactionRepository _transactionRepository;
        private readonly VoucherPrefix _voucherPrefixOptions;
        private readonly IApprovedAmountRepository _approvedAmountRepository;
        private readonly ApplicationStatus _applicationStatusOptions;
        private readonly FinancialTransactionTypes _transactionTypeOptions;
        private readonly BillTypes _billTypeOptions;
        private readonly IBillRepository _billRepository;
        private readonly IVoucherDataRepository _voucherDataRepository;
        private readonly ICalculatorRepository _calculatorRepository;
        private readonly OtherOptions _otherOptions;

        public VouchersController(
            IVoucherRepository voucherBuilder,
            IFinancialTransactionRepository transactionRepository,
            IOptions<VoucherPrefix> voucherPrefixOptions,
            IApprovedAmountRepository approvedAmountRepository,
            IOptions<ApplicationStatus> applicationStatusOptions,
            IOptions<FinancialTransactionTypes> transactionTypeOptions,
            IOptions<BillTypes> billTypeOptions,
            IBillRepository billRepository,
            IVoucherDataRepository voucherDataRepository,
            ICalculatorRepository calculatorRepository,
            IOptions<OtherOptions> otherOptions)
        {
            _voucherBuilder = voucherBuilder;
            _transactionRepository = transactionRepository;
            _voucherPrefixOptions = voucherPrefixOptions.Value;
            _approvedAmountRepository = approvedAmountRepository;
            _applicationStatusOptions = applicationStatusOptions.Value;
            _transactionTypeOptions = transactionTypeOptions.Value;
            _billTypeOptions = billTypeOptions.Value;
            _billRepository = billRepository;
            _voucherDataRepository = voucherDataRepository;
            _calculatorRepository = calculatorRepository;
            _otherOptions = otherOptions.Value;
        }
        #endregion


        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(string id) =>
           Ok(await _voucherDataRepository.GetAllSubByVoucherNo(id));
      
        [HttpGet("reference/{referenceId}")]
        public async Task<IActionResult> GetAllSubByReferenceId(string referenceId) =>
            Ok(await _voucherDataRepository.GetAllSubByReferenceIdAsync(referenceId));


        [HttpGet("job/{jobNo}")]
        public async Task<IActionResult> GetByJobNo(string jobNo) =>
            Ok(await _voucherDataRepository.GetByJobNoAsync(jobNo));

        private async Task<(IActionResult ActionResult, bool IsValid)> ValidateInputAsync(VoucherWithFinancialTransactionDto voucherDto,
            (decimal ApprovedAmount, decimal DueAmount) approvedAndDueAmount, IEnumerable<TransactionDetailsViewModel> transactions)
        {
            if (_applicationStatusOptions.Disbursed ==
                await _approvedAmountRepository.GetApplicationStatusAsync(voucherDto.ApprovalPurposeId, voucherDto.ReferenceId))
                return (Error.BadRequest($"Sorry, already {nameof(_applicationStatusOptions.Disbursed)}."), false);

            if (voucherDto.VoucherDetails.Any(v => v.Dr_amount == 0 && v.Cr_amount == 0))
                return (Error.BadRequest("Sorry, no debit or credit amount found."), false);

            if (voucherDto.VoucherDetails.Sum(v => v.Dr_amount) != voucherDto.VoucherDetails.Sum(v => v.Cr_amount))
                return (Error.BadRequest("Sorry, no debit or credit amount found."), false);

            if (voucherDto.HasDuplicateAccountCode())
                return (Error.BadRequest("Sorry, duplicate postable account head found."), false);

            if (!voucherDto.IsEqualDebitAndCreditAmount())
                return (Error.BadRequest("Sorry, dr side and cr side amount are not equal."), false);

            if ((((voucherDto.Cr + voucherDto.OtherDeduction) > approvedAndDueAmount.DueAmount && approvedAndDueAmount.DueAmount != 0)
      || ((voucherDto.Cr + voucherDto.OtherDeduction) > approvedAndDueAmount.ApprovedAmount && approvedAndDueAmount.DueAmount == 0))
     && (voucherDto.VoucherType == _voucherPrefixOptions.PaymentCash
         || voucherDto.VoucherType == _voucherPrefixOptions.PaymentBank))
                return (Error.BadRequest($"Sorry, you are trying to pay more than {approvedAndDueAmount.DueAmount}."), false);

            if (voucherDto.Dr > Math.Abs(approvedAndDueAmount.DueAmount)
                && (voucherDto.VoucherType == _voucherPrefixOptions.ReceiveCash
                || voucherDto.VoucherType == _voucherPrefixOptions.ReceiveBank))
                return (Error.BadRequest($"Sorry, you are trying to receive more than {approvedAndDueAmount.DueAmount}."), false);

            if (voucherDto.VoucherType == _voucherPrefixOptions.Journal)
            {
                if (!_otherOptions.CanCreateJournalAfterAnyReceiveOrPaymentVoucher)
                    if (transactions.Any())
                        return (Error.BadRequest($"Sorry, you have already voucher entries. So, you should entry journals first."), false);

                var totalAdvanceAmount =
                    await _calculatorRepository.CalculateTotalApprovedAdvanceAmountByJobWorOrder(voucherDto.JobId, voucherDto.WorkOrderMainId);

                var maxAmountBetweenAdvanceAndApproved = Math.Max(totalAdvanceAmount, approvedAndDueAmount.ApprovedAmount);

                if ((voucherDto.Cr + voucherDto.OtherDeduction) != maxAmountBetweenAdvanceAndApproved)
                    return (Error.BadRequest($"Sorry, you have to entry journal amount of {maxAmountBetweenAdvanceAndApproved}."), false);
            }

            return (Ok(), true);
        }

        private async Task SaveReceiveTransactionAsync(VoucherWithFinancialTransactionDto voucherDto)
        {
            if (voucherDto.FinancialTransactionTypeId == _transactionTypeOptions.Bill)
            {
                await _billRepository.ChangeReceivedAmountAsync(voucherDto.ReferenceId, voucherDto.Dr, User.Identity.Name);
                await InsertFinancialTransactionAsync(voucherDto, crAmount: voucherDto.Dr,
                    billType: _billTypeOptions.Received);
            }
            else
                await InsertFinancialTransactionAsync(voucherDto, crAmount: voucherDto.Dr);
        }

        private async Task SavePaymentTransactionAsync(VoucherWithFinancialTransactionDto voucherDto, (decimal ApprovedAmount, decimal DueAmount) approvedAndDueAmount, IEnumerable<TransactionDetailsViewModel> transactions)
        {
            if (voucherDto.FinancialTransactionTypeId == _transactionTypeOptions.Bill)
            {
                var totalAdvanceAmount = await _calculatorRepository.CalculateTotalApprovedAdvanceAmountByJobWorOrder(voucherDto.JobId, voucherDto.WorkOrderMainId);

                if (totalAdvanceAmount == 0 && !transactions.Any(t => t.IsForAdjustment))
                {
                    await InsertFinancialTransactionAsync(voucherDto, crAmount: voucherDto.Cr);
                    await this.ChangeApplicationStatusAsync(voucherDto, User.Identity.Name, approvedAndDueAmount.ApprovedAmount, voucherDto.Cr == 0 ? voucherDto.Dr : voucherDto.Cr);

                    if (voucherDto.OtherDeduction > 0)
                        await InsertFinancialTransactionAsync(voucherDto, crAmount: voucherDto.OtherDeduction, isDeductionEntry: true, accountHead: _otherOptions.OtherDeduction);
                }
                else
                {
                    await this.ChangeApplicationStatusAsync(voucherDto, User.Identity.Name, approvedAndDueAmount.DueAmount, voucherDto.Cr == 0 ? voucherDto.Dr : voucherDto.Cr);
                }
                await InsertFinancialTransactionAsync(voucherDto, drAmount: voucherDto.Dr, billType: _billTypeOptions.Payment);

                if (voucherDto.OtherDeduction > 0)
                    await InsertFinancialTransactionAsync(voucherDto, drAmount: voucherDto.OtherDeduction, isDeductionEntry: true, accountHead: _otherOptions.OtherDeduction);
            }
            else
            {
                await InsertFinancialTransactionAsync(voucherDto, drAmount: voucherDto.Dr);
                await this.ChangeApplicationStatusAsync(voucherDto, User.Identity.Name, approvedAndDueAmount.DueAmount, voucherDto.Cr == 0 ? voucherDto.Dr : voucherDto.Cr);
            }
        }

        private async Task SaveJournalTransactionAsync(VoucherWithFinancialTransactionDto voucherDto, (decimal ApprovedAmount, decimal DueAmount) approvedAndDueAmount)
        {
            voucherDto.VoucherDetails
                .Where(w => w.IsDeductionEntry)
                .ToList()
                           .ForEach(async v =>
                           {
                               await InsertFinancialTransactionAsync(voucherDto, drAmount: v.Cr_amount, isDeductionEntry: true, accountHead: v.AccountHead);
                           });

            if (voucherDto.OtherDeduction > 0)
                await InsertFinancialTransactionAsync(voucherDto, drAmount: voucherDto.OtherDeduction, isDeductionEntry: true, accountHead: _otherOptions.OtherDeduction);

            await InsertFinancialTransactionAsync(voucherDto, crAmount: approvedAndDueAmount.ApprovedAmount, isForAdjustment: true);
        }

        private async Task<(decimal ApprovedAmount, decimal DueAmount)>
            GetApprovedAndDueAmount(VoucherWithFinancialTransactionDto voucherDto, IEnumerable<TransactionDetailsViewModel> transactions)
        {
            decimal dueAmount;

            var approvedAmount = await this._approvedAmountRepository.GetApprovedAmountAsync(voucherDto.ApprovalPurposeId, voucherDto.ReferenceId);

            if (voucherDto.FinancialTransactionTypeId == _transactionTypeOptions.Advance)
            {
                dueAmount = approvedAmount - transactions.Sum(s => s.Cr);
            }
            else
            {
                var paidData = await _transactionRepository.GetByJobAndWorkOrderAsync(voucherDto.JobId, voucherDto.WorkOrderMainId);
                dueAmount = paidData.Sum(s => s.Cr - s.Dr);
            }
            return (approvedAmount, dueAmount);
        }


        [HttpPost]
        public async Task<IActionResult> SaveVocuhers([FromBody] VoucherWithFinancialTransactionDto voucherDto)
        {
            voucherDto.SetDefaultValue();

            var transactions = await _transactionRepository.GetByReferenceIdAsync(voucherDto.ReferenceId);

            var approvedAndDueAmount = await GetApprovedAndDueAmount(voucherDto, transactions);

            var inputValidation = await ValidateInputAsync(voucherDto, approvedAndDueAmount, transactions);
            if (!inputValidation.IsValid)
                return inputValidation.ActionResult;
            try
            {
                await this.SaveVouchersAsync(voucherDto);
            }
            catch (Exception exception)
            {
                return Error.BadRequest($"Error ocurred in voucher. {exception.Message}");
            }
            try
            {
                if (voucherDto.VoucherType == _voucherPrefixOptions.PaymentBank
                    || voucherDto.VoucherType == _voucherPrefixOptions.PaymentCash)
                    await SavePaymentTransactionAsync(voucherDto, approvedAndDueAmount, transactions);
                else if (voucherDto.VoucherType == _voucherPrefixOptions.ReceiveCash
                         || voucherDto.VoucherType == _voucherPrefixOptions.ReceiveBank)
                    await SaveReceiveTransactionAsync(voucherDto);
                else
                    await SaveJournalTransactionAsync(voucherDto, approvedAndDueAmount);

                await _transactionRepository.CommitChangesAsync();

                return CreatedAtRoute("GetById", new { id = voucherDto.VoucherNo }, voucherDto);
            }
            catch (Exception exception)
            {
                return Error.BadRequest($"Error ocurred in financial transaction. {exception.Message}");
            }
        }

        private async Task InsertFinancialTransactionAsync(VoucherWithFinancialTransactionDto transactionDto, decimal drAmount = 0, decimal crAmount = 0,
             string accountHead = null, bool isForAdjustment = false, bool isDeductionEntry = false, string billType = null)
        {
            billType = billType ?? _billTypeOptions.None;
            var transaction = Mapper.Map<FinancialTransactionDetails>(transactionDto);
            transaction.IsForAdjustment = isForAdjustment;
            transaction.IsDeductionEntry = isDeductionEntry;
            transaction.CreatedBy = User.Identity.Name;
            transaction.Dr = drAmount;
            transaction.Cr = crAmount;
            transaction.AccountHead = accountHead;
            transaction.BillType = billType;
            await _transactionRepository.InsertNewAsync(transaction);
        }

        private async Task ChangeApplicationStatusAsync(VoucherWithFinancialTransactionDto transactionDto, string userId, decimal approvedAmount, decimal amountToPay)
        {
            amountToPay += transactionDto.OtherDeduction;
            var applicationStatus = _applicationStatusOptions.NotDisbursed;

            if (approvedAmount > amountToPay)
                applicationStatus = _applicationStatusOptions.PartialyDisbursed;
            else if (approvedAmount == amountToPay)
                applicationStatus = _applicationStatusOptions.Disbursed;

            await this._approvedAmountRepository.ChangeApplicationStatusAsync(transactionDto.ApprovalPurposeId,
                transactionDto.ReferenceId, applicationStatus, userId, amountToPay);
        }

        private async Task SaveVouchersAsync(VoucherWithFinancialTransactionDto voucherDto)
        {
            var voucherMainToSave = Mapper.Map<Accounts_VoucherMaster>(voucherDto);
            voucherMainToSave.UserID = User.Identity.Name;
            await _voucherBuilder.InsertNewAsync(voucherMainToSave);
            voucherDto.VoucherNo = voucherMainToSave.Voucher_no;
            voucherMainToSave.Voucher_Code = voucherMainToSave.Voucher_no;
            await _voucherBuilder.CommitChangesAsync();
        }
    }
}