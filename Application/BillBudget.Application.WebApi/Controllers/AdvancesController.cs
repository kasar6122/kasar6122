using System;
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
using Microsoft.Extensions.Options;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/advances")]
    public class AdvancesController : Controller
    {
        private readonly IAdvanceRepository _advanceRepository;
        private readonly ApplicationStatus _applicationStatus;
        private readonly IBillBudgetEmailSender _billBudgetEmailSender;
        private readonly ICalculatorRepository _calculatorRepository;
        private readonly IJobRepository _jobRepository;
        private readonly OtherOptions _otherOptions;

        public AdvancesController(IAdvanceRepository advanceRepository,
           IOptions<ApplicationStatus> applicationStatusOptions,
            IBillBudgetEmailSender billBudgetEmailSender,
            ICalculatorRepository calculatorRepository,
            IOptions<OtherOptions> otherOptions,
            IJobRepository jobRepository)
        {
            _advanceRepository = advanceRepository;
            _applicationStatus = applicationStatusOptions.Value;
            _billBudgetEmailSender = billBudgetEmailSender;
            _calculatorRepository = calculatorRepository;
            _jobRepository = jobRepository;
            _otherOptions = otherOptions.Value;
        }

        [HttpGet("{id}", Name = "GetAdvanceById")]
        public async Task<IActionResult> GetAdvanceById(string id) => Ok(await _advanceRepository.GetAdvancesByIdAsync(id));


        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetAdvancesByJobId(string jobId) => Ok(await _advanceRepository.GetAdvancesByJobIdAsync(jobId));


        [HttpGet("applicationStatus/{applicationStatus}/approvalStatus/{approvalStatus}")]
        public async Task<IActionResult> GetByApplicationApprovalStatus(string applicationStatus, bool approvalStatus)
            => Ok(await _advanceRepository.GetByApplicationApprovalStatus(applicationStatus, approvalStatus));


        [HttpGet("applicationStatus/{applicationStatus}/approvalStatus/{approvalStatus}/assignedTo/{assignedTo}")]
        public async Task<IActionResult> GetByApplicationApprovalStatus(string applicationStatus, bool approvalStatus, string assignedTo)
            => Ok(await _advanceRepository.GetByApplicationApprovalStatus(applicationStatus, approvalStatus, assignedTo));


        [HttpGet("{id}/job/{jobId}")]
        public async Task<IActionResult> GetAdvances(string id, string jobId)
            => Ok(await _advanceRepository.GetAdvancesByJobIdAsync(id, jobId));


        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] AdvanceMainCreatingDto advanceDto)
        {
            if (advanceDto.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate advance items found.");

            advanceDto.SetDefaultValue();

            if (!string.IsNullOrEmpty(advanceDto.WorkOrderMainId))
            {
                var workOrderAmount = await _calculatorRepository.CalculateTotalWorkOrderAmountByJobWorkOrderId(advanceDto.JobId, advanceDto.WorkOrderMainId);

                if (advanceDto.TotalAmount > workOrderAmount)
                    return Error.BadRequest($"Sorry, you cannot apply for more than {workOrderAmount}.");
            }

            return await this.InsertAdvanceAsync(advanceDto);
        }

        private async Task<IActionResult> InsertAdvanceAsync(AdvanceMainCreatingDto advanceDto)
        {
            if (!await _jobRepository.IsThisJobAssignedToThisUser(advanceDto.JobId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person.");

            advanceDto.SetDefaultValue();

            var advanceToSave = Mapper.Map<AdvanceMain>(advanceDto);
            advanceToSave.CreatedBy = User.Identity.Name;
            advanceToSave.AdvanceDate = DateTime.UtcNow;
            advanceToSave.ApplicationStatus = _applicationStatus.NotDisbursed;

            await _advanceRepository.InsertNewAsync(advanceToSave);
            await _advanceRepository.CommitChangesAsync();
            try
            {
                var url = $"{Request.Headers["Referer"]}{_otherOptions.AdvanceApprovalProcessUrl}{advanceToSave.ApprovalId}/{advanceToSave.JobId}/{advanceToSave.Id}";
                await _billBudgetEmailSender.SendEmailAsync(advanceToSave.ForwardTo,
                    url, $"Advance-{advanceToSave.AdvanceName} Created");
            }
            catch (Exception)
            {
                return Error.BadRequest("Sorry, data saved successfully but falied to send email.");
            }

            var advaceToView = Mapper.Map<AdvanceMainViewModel>(advanceToSave);
            return CreatedAtAction("GetAdvanceById", new { id = advanceToSave.Id }, advaceToView);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> SaveAsync(string id, [FromBody] AdvanceMainUpdatingDto advance)
        {
            var loggedInUser = User.Identity.Name;

            if (!await _jobRepository.IsThisJobAssignedToThisUser(advance.JobId, loggedInUser))
                return Error.BadRequest($"Sorry, you are not valid person.");

            if (await _advanceRepository.IsApprovedAsync(advance.Id))
                return Error.BadRequest($"Sorry, already approved. You cannot update.");

            if (!await _advanceRepository.IsValidAssignedOrForwaredPersonAsync(advance.Id, loggedInUser))
                return Error.BadRequest($"Sorry, you are not assigned person. You cannot update.");

            if (advance.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate advance items found.");

            advance.SetDefaultValue();

            var advanceToSave = Mapper.Map<AdvanceMain>(advance);
            advanceToSave.UpdatedBy = loggedInUser;

            _advanceRepository.Modify(advanceToSave);
            await _advanceRepository.CommitChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvanceWithSub(string id)
        {
            var loggedInUser = User.Identity.Name;
           if(!await _advanceRepository.IsApprovedAsync(id) && await _advanceRepository.IsValidAssignedOrForwaredPersonAsync(id, loggedInUser))
           {
                await _advanceRepository.DeleteAdvanceAsync(id);
                await _advanceRepository.CommitChangesAsync();
                return Ok("Data deleted Successfully!!");
           }
            else
            {
                return Error.BadRequest($"Sorry!! Already Approved or you are not assign person");
            } 
            
            
            
        }



    }
}