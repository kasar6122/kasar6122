using System;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Logic.Abstractions;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/advanceSubs")]
    public class AdvanceSubsController : Controller
    {
        private readonly IAdvanceRepository _advanceRepository;
        private readonly IAdvanceSubRepository _advanceSubRepository;
        private readonly IApprovalRepository _approvalRepository;

        public AdvanceSubsController(IAdvanceRepository advanceRepository,
            IAdvanceSubRepository advanceSubRepository, 
            IApprovalRepository approvalRepository)
        {
            _advanceRepository = advanceRepository;
            _advanceSubRepository = advanceSubRepository;
            _approvalRepository = approvalRepository;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAync(string id, [FromBody]AdvanceSubCreatingDto advanceSub)
        {
           var loggedInUser = User.Identity.Name;
            if (await _advanceRepository.IsApprovedAsync(advanceSub.AdvanceMainId))
                return Error.BadRequest($"Sorry, already approved. You cannot update.");

            //  if (!await _advanceRepository.IsValidAssignedOrForwaredPersonAsync(advanceSub.AdvanceMainId, loggedInUser))
            //  return Error.BadRequest($"Sorry, you are not assigned person. You cannot update.");

            if (!await _advanceRepository.IsValidAssignedOrForwaredPersonAsync(advanceSub.AdvanceMainId, loggedInUser))
                if (!await _approvalRepository.CanUpdateAsync(loggedInUser, advanceSub.AdvanceMainId))
                    return Error.BadRequest($"Sorry, you are not valid person to update.");

            var advanceToUpdate = Mapper.Map<AdvanceSub>(advanceSub);
            advanceToUpdate.UpdatedBy = loggedInUser;
            advanceToUpdate.UpdatedOn = DateTime.UtcNow;

            await _advanceSubRepository.ModifyAsync(advanceToUpdate);
            await _advanceSubRepository.CommitChangesAsync();
            return NoContent();
        }


        [HttpDelete("{subId}")]
        public async Task<IActionResult> DeleteSubItemAsync(string subId)
        {
            await _advanceSubRepository.DeleteAdvanceSubAsync(subId);
            await _advanceSubRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}