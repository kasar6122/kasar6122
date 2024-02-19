using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Helpers;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Domain.Helpers;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.ViewModels;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/budgets")]
    public class BudgetsController : Controller
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IJobRepository _jobRepository;
        private readonly BudgetPerformTypes _performanceOptions;
        private readonly ApprovalPurposes _approvalOptions;
        private readonly ApplicationStatus _applicationStatusOptions;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IConfiguration _configurationRoot;
        private readonly IBillBudgetEmailSender _billBudgetEmailSender;
        private readonly IAdvanceRepository _advanceRepository;
        private readonly OtherOptions _otherOptions;

        public BudgetsController(
            IBudgetRepository budgetRepository,
            IJobRepository jobRepository,
            IOptions<BudgetPerformTypes> performanceOptions,
            IOptions<ApprovalPurposes> approvalOptions,
            IApprovalRepository approvalRepository,
            IOptions<ApplicationStatus> applicationStatusOptions,
            IConfiguration configurationRoot,
            IBillBudgetEmailSender billBudgetEmailSender,
            IAdvanceRepository advanceRepository,
            IOptions<OtherOptions> otherOptions)
        {
            this._budgetRepository = budgetRepository;
            this._jobRepository = jobRepository;
            this._performanceOptions = performanceOptions.Value;
            this._approvalOptions = approvalOptions.Value;
            this._approvalRepository = approvalRepository;
            this._applicationStatusOptions = applicationStatusOptions.Value;
            this._configurationRoot = configurationRoot;
            this._billBudgetEmailSender = billBudgetEmailSender;
            this._advanceRepository = advanceRepository;
            this._otherOptions = otherOptions.Value;
        }


        [HttpGet("{id}", Name = "GetBudget")]
        public async Task<IActionResult> GetBudget(string id) =>
            Ok(await _budgetRepository.GetById(id));


        [HttpGet("costCenter/{CostCenterId}")]
        public async Task<IActionResult> GetByCostCenterId(string costCenterId) =>
            Ok(await _budgetRepository.GetByCostCenterId(costCenterId));


        [HttpGet("assignTo/{assignTo}")]
        public async Task<IActionResult> GetByAssignTo(string assignTo) =>
            Ok(await _budgetRepository.GetByAssignTo(assignTo));


        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJobId(string jobId) =>
            Ok(await _budgetRepository.GetByJobId(jobId));


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (await _approvalRepository.IsApprovedAsync(id, _approvalOptions.Budget, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, already {nameof(_performanceOptions.Approved)}. You cannot update.");

            var loggedInUser = User.Identity.Name;
            if (!await this._budgetRepository.IsValidPerson(loggedInUser, id))
                return Error.BadRequest("Sorry, you are not assigned person to this job.");

            await _budgetRepository.DeleteAsync(id);
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] BudgetMainCreatingDto budgetCreatingDto)
        {
            var loggedInUser = User.Identity.Name;

            budgetCreatingDto.SetDefaultValue();

            if (budgetCreatingDto.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate budget items found.");

            if (!await this._jobRepository.IsThisJobAssignedToThisUser(budgetCreatingDto.JobId, loggedInUser))
                return Error.BadRequest("Sorry, you are not assigned person to this job.");

            if (await _budgetRepository.IsThisJobHasActiveBudgetAsync(budgetCreatingDto.JobId))
            {
                var advances = await _advanceRepository.GetAdvancesByJobIdAsync(budgetCreatingDto.JobId);
                var totalApprovedAdvance = advances.Sum(a => a.TotalApproveAmount);
                if (budgetCreatingDto.TotalBudgetAmount >= totalApprovedAdvance)
                    await _budgetRepository.InactiveAllActiveBudgetsOfAJob(budgetCreatingDto.JobId, loggedInUser);
                else
                    return Error.BadRequest($"Sorry, this job has already an active budget and total approved advance amount of {totalApprovedAdvance}. You have to create a budget of more than or equal {totalApprovedAdvance}");
            }
            
            var budgetToSave = Mapper.Map<BudgetMain>(budgetCreatingDto);
            budgetToSave.CreatedBy = loggedInUser;
            budgetToSave.ApplicationStatus = _applicationStatusOptions.NotDisbursed;

            await _budgetRepository.InsertNewAsync(budgetToSave);
            await _budgetRepository.CommitChangesAsync();

            var budgetToView = Mapper.Map<BudgetMainViewModel>(budgetToSave);
            try
            {
                var url = $"{Request.Headers["Referer"]}{_otherOptions.BudgetApprovalProcessUrl}{budgetToSave.ApprovalId}";
                await _billBudgetEmailSender
                    .SendEmailAsync(budgetCreatingDto.ForwardTo, url, $"Budget-{budgetToSave.BudgetName} Created");
            }
            catch (Exception)
            {
                return Error.BadRequest("Sorry, data saved successfully but falied to send email.");
            }

            return CreatedAtRoute("GetBudget", new { id = budgetToSave.Id }, budgetToView);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]BudgetMainUpdatingDto budgetUpdatingDto)
        {
            budgetUpdatingDto.SetDefaultValue();
            if (!budgetUpdatingDto.BudgetSub.Any())
                return Error.BadRequest("Sorry, no budget items found to save.");

            if (budgetUpdatingDto.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate budget items found.");

            if (await _approvalRepository.IsApprovedAsync(budgetUpdatingDto.Id, _approvalOptions.Budget, _performanceOptions.Approved))
                return Error.BadRequest($"Sorry, {budgetUpdatingDto.BudgetName} already approved. You cannot update.");

            var loggedInUser = User.Identity.Name;
            if (!await this._budgetRepository.IsValidPerson(loggedInUser, budgetUpdatingDto.Id))
                return Error.BadRequest("Sorry, you are not assigned person to this job.");

            var budgetToSave = Mapper.Map<BudgetMain>(budgetUpdatingDto);

            budgetToSave.UpdatedBy = loggedInUser;
            budgetToSave.UpdatedOn = DateTime.UtcNow;

            _budgetRepository.UpdateBudget(budgetToSave);
            await _budgetRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}