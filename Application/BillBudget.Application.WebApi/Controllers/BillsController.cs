using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Helpers;
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
    [Route("api/bills")]
    public class BillsController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly BudgetPerformTypes _performance;
        private readonly ApprovalPurposes _approvalOptions;
        private readonly IApprovalRepository _approvalRepository;
        private readonly ApplicationStatus _applicationStatus;
        private readonly IBillBudgetEmailSender _billBudgetEmailSender;
        private readonly IJobRepository _jobRepository;
        private readonly OtherOptions _otherOptions;

        public BillsController(IBillRepository billRepository,
            IOptions<BudgetPerformTypes> performanceOptions,
            IOptions<ApprovalPurposes> approvalOptions,
            IApprovalRepository approvalRepository,
            IOptions<ApplicationStatus> applicationStatusOptions,
            IBillBudgetEmailSender billBudgetEmailSender,
            IOptions<OtherOptions> otherOptions,
            IJobRepository jobRepository)
        {
            this._billRepository = billRepository;
            _performance = performanceOptions.Value;
            _approvalOptions = approvalOptions.Value;
            _approvalRepository = approvalRepository;
            _applicationStatus = applicationStatusOptions.Value;
            _billBudgetEmailSender = billBudgetEmailSender;
            _jobRepository = jobRepository;
            _otherOptions = otherOptions.Value;
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJobId(string jobId) =>
            Ok(await _billRepository.GetByJobIdAsync(jobId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => Ok(await _billRepository.GetByIdAsync(id));


        [HttpGet("{billId}/job/{jobId}")]
        public async Task<IActionResult> GetByJobId(string billId, string jobId) => Ok(await _billRepository.GetByIdJobIdAsync(billId, jobId));

        [HttpGet("applicationStatus/{applicationStatus}/approvalStatus/{approvalStatus}")]
        public async Task<IActionResult> GetByApplicationApprovalStatus(string applicationStatus, bool approvalStatus)
            => Ok(await _billRepository.GetByApplicationApprovalStatus(applicationStatus, approvalStatus));


        [HttpGet("applicationStatus/{applicationStatus}/approvalStatus/{approvalStatus}/assignedTo/{assignedTo}")]
        public async Task<IActionResult> GetByApplicationApprovalStatus(string applicationStatus, bool approvalStatus, string assignedTo)
            => Ok(await _billRepository.GetByApplicationApprovalStatus(applicationStatus, approvalStatus, assignedTo));


        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody]BillMainCreatingDto billDto)
        {
            if (!await _jobRepository.IsThisJobAssignedToThisUser(billDto.JobId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person.");

            billDto.SetDefaultValue();

            if (await _billRepository.HasThisJobAndWorkOrderAnyBill(billDto.JobId, billDto.WorkOrderMainId))
                return Error.BadRequest($"Sorry, you have created bill for this job.");

            if (billDto.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate bill items found.");

            var billToSave = Mapper.Map<BillMain>(billDto);
            billToSave.TotalReceivedAmount = 0;
            billToSave.CreatedBy = User.Identity.Name;
            billToSave.BillDate = DateTime.UtcNow;
            billToSave.ApplicationStatus = _applicationStatus.NotDisbursed;

            await _billRepository.InsertNewAsync(billToSave);
            await _billRepository.CommitChangesAsync();

            try
            {
                var url = $"{Request.Headers["Referer"]}{_otherOptions.BillApprovalProcessUrl}{billToSave.ApprovalId}";
                await _billBudgetEmailSender
                    .SendEmailAsync(billToSave.ForwardTo, url, $"Bill-{billToSave.BillName} Created");
            }
            catch (Exception)
            {
                return Error.BadRequest("Sorry, data saved successfully but falied to send email.");
            }

            var billToView = Mapper.Map<BillMainUpdatingDto>(billToSave);
            return CreatedAtAction("GetById", new { id = billToSave.Id }, billToView);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] BillMainUpdatingDto billDto)
        {
            if (!await _jobRepository.IsThisJobAssignedToThisUser(billDto.JobId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person.");
            
            if (!billDto.BillSub.Any())
                return Error.BadRequest("Sorry, no bill items found to save.");

            if (billDto.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate bill items found.");

            if (await _approvalRepository.IsApprovedAsync(billDto.Id, _approvalOptions.Bill, _performance.Approved))
                return Error.BadRequest($"Sorry, already approved. You cannot update.");

            if (!await _billRepository.IsValidAssignedForwardedPerson(billDto.Id, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person to update.");

            billDto.SetDefaultValue();

            var billToSave = Mapper.Map<BillMain>(billDto);
            billToSave.UpdatedBy = User.Identity.Name;
            billToSave.UpdatedOn = DateTime.UtcNow;
            billToSave.TotalReceivedAmount = 0;

            _billRepository.Modify(billToSave);
            await _billRepository.CommitChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
       {

            if (await _approvalRepository.IsApprovedAsync(id, _approvalOptions.Bill, _performance.Approved))
                return Error.BadRequest($"Sorry, already approved. You cannot update.");


            if (!await _billRepository.IsValidAssignedForwardedPerson(id, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person to update.");

            await _billRepository.DeleteAsync(id);
            await _billRepository.CommitChangesAsync();
            return NoContent();
        }

    }
}