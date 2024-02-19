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
    [Route("api/budgetSubs")]
    public class BudgetSubsController : Controller
    {
        private readonly IBudgetSubRepository _budgetSubRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly ApprovalPurposes _approvalOptions;
        private readonly BudgetPerformTypes _performanceOptions;
        public BudgetSubsController(IBudgetSubRepository budgetSubRepository,
            IApprovalRepository approvalRepository,
            IOptions<ApprovalPurposes> approvalOptions,
            IOptions<BudgetPerformTypes> performanceOptions,
            IBudgetRepository budgetRepository)
        {
            _budgetSubRepository = budgetSubRepository;
            _approvalRepository = approvalRepository;
            _budgetRepository = budgetRepository;
            _approvalOptions = approvalOptions.Value;
            _performanceOptions = performanceOptions.Value;
        }


        [HttpDelete("{id}/budgetMain/{budgetMainId}")]
        public async Task<IActionResult> DeleteAync(string id, string budgetMainId)
        {
            if (await _approvalRepository.IsApprovedAsync(budgetMainId, _approvalOptions.Budget, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, budget already approved. You cannot delete.");

            var loggedInUser = User.Identity.Name;
            if (!await this._approvalRepository.CanUpdateAsync(loggedInUser, budgetMainId))
                return Error.BadRequest($"Sorry, you are not valid person to update");

            await _budgetSubRepository.DeleteBudgetSub(id);
            await _budgetSubRepository.CommitChangesAsync();
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAync(string id, [FromBody]BudgetSubUpdatingDto budgetSub)
        {
            var loggedInUser = User.Identity.Name;
            //if (!await this._approvalRepository.IsValidPersonAsync(loggedInUser, budgetSub.BudgetMainId))
            //    return Error.BadRequest($"Sorry, you are not assigned person");

            if (!budgetSub.IsValidAmount())
                return Error.BadRequest($"Sorry, you cannot approve more than {budgetSub.BudgetAmount}");

            if (await _approvalRepository.IsApprovedAsync(budgetSub.BudgetMainId, _approvalOptions.Budget, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, budget already approved. You cannot update.");

            if (!await this._budgetRepository.IsValidPerson(loggedInUser, budgetSub.BudgetMainId))
                if (!await _approvalRepository.CanUpdateAsync(loggedInUser, budgetSub.BudgetMainId))
                    return Error.BadRequest($"Sorry, you are not valid person to update.");

            var budgetSubToUpdate = Mapper.Map<BudgetSub>(budgetSub);
            budgetSubToUpdate.UpdatedBy = loggedInUser;
            budgetSubToUpdate.UpdatedOn = DateTime.UtcNow;

            await _budgetSubRepository.ModifyAsync(budgetSubToUpdate);
            await _budgetSubRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}