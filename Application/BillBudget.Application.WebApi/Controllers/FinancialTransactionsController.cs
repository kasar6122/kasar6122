using System.Threading.Tasks;
using BillBudget.Core.Domain.Helpers;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/financialTransactions")]
    public class FinancialTransactionsController : Controller
    {
        private readonly IFinancialTransactionRepository _transactionRepository;
        private readonly IApprovedAmountRepository _approvedAmountRepository;
        private readonly ApplicationStatus _applicationStatusOptions;
        private readonly FinancialTransactionTypes _transactionTypeOptions;
        private readonly IAdvanceRepository _advanceRepository;
        public FinancialTransactionsController(IFinancialTransactionRepository transactionRepository,
            IApprovedAmountRepository approvedAmountRepository,
            IOptions<ApplicationStatus> applicationStatusOptions,
            IOptions<FinancialTransactionTypes> transactionTypeOptions,
            IAdvanceRepository advanceRepository)
        {
            _transactionRepository = transactionRepository;
            _approvedAmountRepository = approvedAmountRepository;
            _applicationStatusOptions = applicationStatusOptions.Value;
            _transactionTypeOptions = transactionTypeOptions.Value;
            _advanceRepository = advanceRepository;
        }


       [HttpGet("{id}", Name = "FindById")]
        public async Task<IActionResult> FindById(string id) => Ok(await _transactionRepository.GetByIdAsync(id));


        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJobIdAsync(string jobId) => Ok(await _transactionRepository.GetByJobIdAsync(jobId));


        [HttpGet("job/{jobId}/transactionType/{transactionTypeId}")]
        public async Task<IActionResult> GetByJobIdAsync(string jobId, string transactionTypeId) =>
            Ok(await _transactionRepository.GetByJobAndTransactionTypeAsync(jobId, transactionTypeId));


        [HttpGet("transactionType/{transactionTypeId}/job/{jobId}/workOrder/{workOrderId?}")]
        public async Task<IActionResult> GetByTransactionTypeJobAndWorkOrderAsync(string transactionTypeId, string jobId, string workOrderId) =>
            Ok(await _transactionRepository.GetByTransactionTypeJobAndWorkOrderAsync(transactionTypeId, jobId, workOrderId));


        [HttpGet("reference/{refrenceId}")]
        public async Task<IActionResult> GetByReferenceIdAsync(string refrenceId) =>
            Ok(await _transactionRepository.GetByReferenceIdAsync(refrenceId));


        [HttpGet("assignedTo/{assignedTo}")]
        public async Task<IActionResult> CalculateTotalUnadjustedAdvance(string  assignedTo)
            => Ok(await _transactionRepository
                .CountTotalUnadjustedAdvanceAsync(assignedTo));


        [HttpGet("voucher/{voucherNo}")]
        public async Task<IActionResult> GetByVoucherNoAsync(string voucherNo) =>
            Ok(await _transactionRepository.GetByVoucherNoAsync(voucherNo));


        [HttpGet("transactionType/{transactionTypeId}")]
        public async Task<IActionResult> GetByTransactionTypeIdAsync(string transactionTypeId) =>
            Ok(await _transactionRepository.GetByTransactionTypeIdAsync(transactionTypeId));


        //[HttpPost]
        //public async Task<IActionResult> Save([FromBody] TransactionDetailsDto transactionDto)
        //{
        //    var userId = User.Identity.Name;

        //    var transactionToSave = Mapper.Map<FinancialTransactionDetails>(transactionDto);
        //    transactionToSave.CreatedBy = userId;

        //    if (transactionDto.FinancialTransactionTypeId == _transactionTypeOptions.Bill)
        //    {
        //        var advancesByJobId = await _advanceRepository.GetAdvancesByJobIdAsync(transactionToSave.JobId);
        //        if (!advancesByJobId.Any())
        //            await _transactionRepository.InsertNewAsync(transactionToSave);
        //    }

        //    await _transactionRepository.InsertNewAsync(transactionToSave);

        //    await ChangeApplicationStatus(transactionDto, userId, transactionToSave.Cr == 0 ? transactionToSave.Dr : transactionToSave.Cr);

        //    await _transactionRepository.CommitChangesAsync();

        //    var transactionToView = Mapper.Map<TransactionDetailsViewModel>(transactionToSave);
        //    return CreatedAtAction("FindById", new { id = transactionToSave.Id }, transactionToView);
        //}

        //private async Task ChangeApplicationStatus(TransactionDetailsDto transactionDto, string userId, decimal paidAmount)
        //{
        //    var approvedAmount =
        //        await this._approvedAmountRepository.GetApprovedAmountAsync(transactionDto.ApprovalPurposeId, transactionDto.ReferenceId);
        //    var amountToPay = transactionDto.Cr == 0 ? transactionDto.Dr : transactionDto.Cr;
        //    var applicationStatus = _applicationStatusOptions.Disbursed;

        //    if (approvedAmount > amountToPay)
        //        applicationStatus = _applicationStatusOptions.PartialyDisbursed;

        //    await this._approvedAmountRepository.ChangeApplicationStatusAsync(transactionDto.ApprovalPurposeId,
        //        transactionDto.ReferenceId, applicationStatus, userId, paidAmount);
        //}
    }
}