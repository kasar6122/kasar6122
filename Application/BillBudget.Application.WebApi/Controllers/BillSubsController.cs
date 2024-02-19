using System;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Domain.Helpers;
using BillBudget.Core.Logic.Abstractions;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/billSubs")]
    public class BillSubsController : Controller
    {
        private readonly IBillSubRepository _billSubRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly ApprovalPurposes _approvalOptions;
        private readonly BudgetPerformTypes _performanceOptions;
        private readonly IBillRepository _billRepository;
        private readonly IJobRepository _jobRepository;

        public BillSubsController(IBillSubRepository budgetSubRepository,
            IApprovalRepository approvalRepository,
            IOptions<ApprovalPurposes> approvalOptions,
            IOptions<BudgetPerformTypes> performanceOptions,
            IBillRepository billRepository,
            IJobRepository jobRepository)
        {
            _billSubRepository = budgetSubRepository;
            _approvalRepository = approvalRepository;
            _approvalOptions = approvalOptions.Value;
            _performanceOptions = performanceOptions.Value;
            _billRepository = billRepository;
            _jobRepository = jobRepository;
        }

        [HttpDelete("{id}/billMain/{billMainId}")]
        public async Task<IActionResult> DeleteAync(string id, string billMainId)
        {
            if (await _approvalRepository.IsApprovedAsync(billMainId, _approvalOptions.Bill, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, already approved. You cannot delete.");

            if (!await _billRepository.IsValidAssignedForwardedPerson(billMainId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person to delete.");

            await _billSubRepository.DeleteBillSubAsync(id);
            await _billSubRepository.CommitChangesAsync();
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAync(string id, [FromBody]BillSubCreatingDto billSub)
        {
            if (await _approvalRepository.IsApprovedAsync(billSub.BillMainId, _approvalOptions.Bill, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, already approved. You cannot update.");

            // var isValidApprovalPerson = await _approvalRepository.IsValidPersonAsync(User.Identity.Name, billSub.BillMainId);
            //if (!await )
            //    return Error.BadRequest($"Sorry, you are not valid person to approve.");
            //var isValidAsssingnedOrForwardedPerson =
            //  await _billRepository.IsValidAssignedForwardedPerson(billSub.BillMainId, User.Identity.Name);

            if (!await _approvalRepository.CanUpdateAsync(User.Identity.Name, billSub.BillMainId))
                if (!await _billRepository.IsValidAssignedForwardedPerson(billSub.BillMainId, User.Identity.Name))
                    return Error.BadRequest($"Sorry, you are not valid person to update.");

            //if (!await _billRepository.IsValidAssignedForwardedPerson(billSub.BillMainId, User.Identity.Name))
            //    return Error.BadRequest($"Sorry, you are not valid person to update.");

            //  if (isValidApprovalPerson || isValidAsssingnedOrForwardedPerson)
            // {
            var billToUpdate = Mapper.Map<BillSub>(billSub);
            billToUpdate.UpdatedBy = User.Identity.Name;
            billToUpdate.UpdatedOn = DateTime.UtcNow;
            await _billSubRepository.ModifyAsync(billToUpdate);
            await _billSubRepository.CommitChangesAsync();
            return NoContent();
            // }

            //return Error.BadRequest($"Sorry, you are not valid person to update.");


        }
    }
}