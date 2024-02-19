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
using DaffodilSoftware.Pagination.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/approvals")]
    public class ApprovalsController : Controller
    {
        private readonly IApprovalRepository _approvalRepository;
        private readonly OtherOptions _otherOptions;
        private readonly BudgetPerformTypes _performanceTypes;
        private readonly IUrlHelper _urlHelper;
        private readonly IApprovalCommitteeRepository _approvalCommitteeRepository;
        private readonly IApprovedAmountRepository _approvedAmountRepository;
        private readonly ApprovalPurposes _approvalPurposes;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IBillBudgetEmailSender _billBudgetEmailSender;
        private readonly ICalculatorRepository _calculatorRepository;
        private readonly ICostCenterRepository _costCenterRepository;

        public ApprovalsController(IApprovalRepository approvalRepository,
            IOptions<BudgetPerformTypes> budgetOptions,
            IOptions<OtherOptions> otherOptions,
            IUrlHelper urlHelper,
            IApprovalCommitteeRepository approvalCommitteeRepository,
            IApprovedAmountRepository approvedAmountRepository,
            IOptions<ApprovalPurposes> approvalPurposeOptions,
            IBudgetRepository budgetRepository,
            IBillBudgetEmailSender billBudgetEmailSender,
            IConfiguration configurationRoot,
            ICalculatorRepository calculatorRepository,
            ICostCenterRepository costCenterRepository)
        {
            _approvalRepository = approvalRepository;
            _otherOptions = otherOptions.Value;
            _performanceTypes = budgetOptions.Value;
            _urlHelper = urlHelper;
            _approvalCommitteeRepository = approvalCommitteeRepository;
            _approvedAmountRepository = approvedAmountRepository;
            _approvalPurposes = approvalPurposeOptions.Value;
            _budgetRepository = budgetRepository;
            _billBudgetEmailSender = billBudgetEmailSender;
            _calculatorRepository = calculatorRepository;
            _costCenterRepository = costCenterRepository;
        }

        [HttpPut]
        public async Task<IActionResult> ApproveUpdate([FromBody] ApprovalCreatingDto approval)
        {
            var loggedInUser = User.Identity.Name;
            var workForId = User.Claims.FirstOrDefault(c => c.Type == "WorkForId")?.Value;

            if (approval.BudgetPerformTypeId == _performanceTypes.Forwarded
                && approval.ForwardTo.Length != loggedInUser.Length)
                return Error.BadRequest("Sorry, valid forward to required.");

            if ((approval.BudgetPerformTypeId == _performanceTypes.Pending && approval.CreatedBy == loggedInUser))
            {
                if (await _approvalRepository.UpdateForwardTo(approval.ForwardTo, approval.Ref_Id, approval.Id,approval.Note))
                {
                    return Ok("updated succesfully");
                }

            }

            return Error.BadRequest($"Sorry, you are not valid person for updating the forwardToDetails.");
        }
    
        [HttpPost]
        public async Task<IActionResult> Approve([FromBody] ApprovalCreatingDto approval)
        {
            var loggedInUser = User.Identity.Name;
            var workForId = User.Claims.FirstOrDefault(c => c.Type == "WorkForId")?.Value;

            if (approval.BudgetPerformTypeId == _performanceTypes.Forwarded
                && approval.ForwardTo.Length != loggedInUser.Length)
                return Error.BadRequest("Sorry, valid forward to required.");

            if (await _approvalRepository.CheckByPerformanceTypeAsync(_performanceTypes.Approved, _performanceTypes.Rejected, _performanceTypes.Pending, approval.Ref_Id, loggedInUser))
                return Error.BadRequest($"Sorry, already approved or rejected or pending.");
            
            //checking for validity of user wheather he has power of appoval or not//
            if (!await _approvalRepository.CanApproveAsync(loggedInUser, approval.Ref_Id))
                return Error.BadRequest($"Sorry, you are not valid person to approve.");

            decimal respectedApprovedAmount = 0;

            if (approval.BudgetPerformTypeId == _performanceTypes.Approved
                || approval.BudgetPerformTypeId == _performanceTypes.Rejected)
            {
                respectedApprovedAmount = await this._approvedAmountRepository.GetApprovedAmountAsync(approval.ApprovalPurposeId, approval.Ref_Id);
                var apporovalLimitAmount = await _approvalCommitteeRepository.GetApprovalLimitAmountAsync(loggedInUser);

                if (respectedApprovedAmount > apporovalLimitAmount)
                    return Error.BadRequest($"Sorry, you cannot cross your approval limit of {apporovalLimitAmount} or you are not in approval committee.");
            }

            if ((approval.ApprovalPurposeId == _approvalPurposes.Advance
                || approval.ApprovalPurposeId == _approvalPurposes.Bill)
                && (approval.BudgetPerformTypeId == _performanceTypes.Approved
                || approval.BudgetPerformTypeId == _performanceTypes.Rejected))
            {
                var activeBudget = await _budgetRepository.GetByJobId(approval.JobId, activeStatus: true);

                if (activeBudget != null)
                {
                    if (!(activeBudget.UpdatedBy == loggedInUser
                       || activeBudget.CreatedBy == loggedInUser
                       || activeBudget.UpdatedBy == workForId
                       || activeBudget.CreatedBy == workForId
                       || _otherOptions
                       .SuperApprovalAuthorities
                       .Split(",")
                       .Any(sa => sa.Replace(" ", "") == loggedInUser
                               || sa.Replace(" ", "") == workForId)))
                    {
                        if (!await _costCenterRepository.IsValidApprovalAuthorityAsync(activeBudget.CostCenterId, loggedInUser))
                            return Error.BadRequest("Sorry, The person who approved the respective budget can only approve related bills and advances.");
                    }

                    if (!activeBudget.IsApproved)
                        return Error.BadRequest("Sorry, budget is not approved.");

                    var totalApprovedAdvanceAmount =
                        await _calculatorRepository.CalculateTotalApprovedAdvanceAmountByJobId(approval.JobId);

                    if (activeBudget.TotalApprovedAmount < totalApprovedAdvanceAmount)
                        return Error.BadRequest($"Sorry your budget approved amount is {activeBudget.TotalApprovedAmount} but you are trying to approve {totalApprovedAdvanceAmount}");
                    var totalApprovedBillAmount =
                        await _calculatorRepository.CalculateTotalApprovedBillAmountByJobId(approval.JobId);
                    if (activeBudget.TotalApprovedAmount < (totalApprovedBillAmount))
                        return Error.BadRequest($"Sorry your budget approved amount is {activeBudget.TotalApprovedAmount} but you are trying to approve Bill {totalApprovedBillAmount}");
                }
                else
                {
                    if(approval.ApprovalPurposeId == _approvalPurposes.Advance)
                    {
                        return Error.BadRequest("Sorry, no active budget found.");
                    }
                }
                    
            }

            if (approval.ApprovalPurposeId == _approvalPurposes.Budget && approval.BudgetPerformTypeId == _performanceTypes.Rejected)
                await _budgetRepository.RejectBudgetAsync(approval.Ref_Id, loggedInUser);

            if (approval.BudgetPerformTypeId == _performanceTypes.Approved)
                await this._approvedAmountRepository.ChangeApprovalStatusAsync(approval.ApprovalPurposeId, approval.Ref_Id, loggedInUser);

            var actionResult = await SaveOrUpdateApproval(approval, loggedInUser);

            try
            {
                var urlAndApprovalType = this.GenerateUrl(approval.ApprovalPurposeId, approval.JobId, approval.Ref_Id);

                if ((approval.BudgetPerformTypeId == _performanceTypes.Forwarded
                     || approval.BudgetPerformTypeId == _performanceTypes.Recommended)
                    && !string.IsNullOrWhiteSpace(approval.ForwardTo))
                    await _billBudgetEmailSender.SendEmailAsync(approval.ForwardTo, urlAndApprovalType.Url, urlAndApprovalType.ApprovalType);
                else if (!string.IsNullOrWhiteSpace(approval.AssignTo))
                    await _billBudgetEmailSender.SendEmailAsync(approval.AssignTo, urlAndApprovalType.Url, urlAndApprovalType.ApprovalType);
            }
            catch (Exception )
            {
                return Error.BadRequest("Approved, but unabled to send email.");
            }

            return actionResult;
        }

        private (string Url, string ApprovalType) GenerateUrl(string approvalPurposeId, string jobId, string referenceId)
        {
            var url = Request.Headers["Referer"];
            var approvalType = "";
            if (approvalPurposeId == _approvalPurposes.Budget)
            {
                approvalType = "Budget Approved";
                url += _otherOptions
                    .BudgetApprovalUrl
                    .Replace("jobId", jobId)
                    .Replace("budgetId", referenceId);
            }
            else if (approvalPurposeId == _approvalPurposes.Advance)
            {
                approvalType = "Advance Approved";
                url += _otherOptions
                    .AdvanceApprovalUrl
                    .Replace("jobId", jobId)
                    .Replace("advanceId", referenceId);
            }
            else if (approvalPurposeId == _approvalPurposes.Bill)
            {
                approvalType = "Bill Approved";
                url += _otherOptions
                    .BillApprovalUrl
                    .Replace("jobId", jobId)
                    .Replace("billId", referenceId);
            }
            else
                url = "";
            return (url, approvalType);
        }

        private async Task<IActionResult> SaveOrUpdateApproval(ApprovalCreatingDto approval, string userId)
        {
            var approvalToUpdate = Mapper.Map<Approval>(approval);
            approvalToUpdate.UpdatedBy = userId;
            approvalToUpdate.BudgetPerformTypeId = _performanceTypes.Forwarded;
            await _approvalRepository.ModifyAsync(approvalToUpdate);

            var approvalToSave = Mapper.Map<Approval>(approval);
            approvalToSave.Id = SingletonIdGenerator.Instance.Id.ToString();
            approvalToSave.CreatedBy = userId;

            if (approval.BudgetPerformTypeId == _performanceTypes.Forwarded)
                approvalToSave.BudgetPerformTypeId = _performanceTypes.Pending;
            else
            {
                approvalToSave.BudgetPerformTypeId = approval.BudgetPerformTypeId;
                approvalToSave.ForwardTo = null;
            }

            await _approvalRepository.InsertNewAsync(approvalToSave);
            await _approvalRepository.CommitChangesAsync();

            return CreatedAtRoute("GetApprovalsByJobId", new { jobId = approvalToSave.JobId }, approval);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) =>
            Ok(await _approvalRepository.GetByIdAsync(id));


        [HttpGet("job/{jobId}", Name = "GetApprovalsByJobId")]
        public async Task<IActionResult> GetApprovalsByJobId(string jobId) =>
            Ok(await _approvalRepository.GetApprovalSummaryByJobId(jobId));


        [HttpGet("reference/{referenceId}")]
        public async Task<IActionResult> GetApprovalSummaryByReferenceId(string referenceId) =>
            Ok(await _approvalRepository.GetApprovalSummaryByReferenceId(referenceId));


        [HttpGet("performanceType/{performanceTypeId}/approvalPurpose/{approvalPurposeId}")]
        public async Task<IActionResult> GetByForwardToPerformanceTypeApprovalPurpose(ApprovalsQueryParams queryParams)
        {
            queryParams.ForwardTo = User.Identity.Name;
            return Ok(await _approvalRepository.GetByForwardToPerformanceTypeApprovalPurpose(queryParams));
        }


        [HttpGet("performanceType/{performanceTypeId}")]
        public async Task<IActionResult> GetByForwardToPerformanceType(ApprovalsQueryParams queryParams)
        {
            queryParams.ForwardTo = User.Identity.Name;
            return Ok(await _approvalRepository.GetByForwardToPerformanceTypeApprovalPurpose(queryParams));
        }


        [HttpGet("job/{jobId}/approvalPurpuse/{approvalPurpuseId}")]
        public async Task<IActionResult> GetApprovalSummary(string jobId, string approvalPurpuseId)
        {
            return Ok(await _approvalRepository.GetApprovalSummaryByJobId(jobId, approvalPurpuseId));
        }


        [HttpGet(Name = "GetAllApprovalsIfAdmin")]
        public async Task<IActionResult> GetAll([FromQuery] ApprovalsQueryParams args)
        {
            PagedList<ApprovalViewModel> approvals;
            if (!User.IsInRole("admin"))
            {
                args.ForwardTo = User.Identity.Name;
                args.PerformanceTypeId = _performanceTypes.Pending;
                approvals = await _approvalRepository.GetByForwardToPerformanceTypeApprovalPurpose(args);
            }
            else
            {
                args.PerformanceTypeId = _performanceTypes.Pending;
                approvals = await _approvalRepository.GetByPerformanceTypeAsync(args);
            }

            var paginationMetadata = new SqlPaginator<ResourceQueryParameters, ApprovalViewModel>(_urlHelper)
                .GetPaginationMetadata("GetAllApprovalsIfAdmin", args, approvals);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(approvals);
        }


        [HttpGet("performanceType/{performanceTypeId}/reference/{referenceId}")]
        public async Task<IActionResult> GetByForwardToPerformanceTypeReferenceId(ApprovalsQueryParams queryParams)
        {
            queryParams.ForwardTo = User.Identity.Name;
            return Ok(await _approvalRepository.GetByForwardToPerformanceTypeReferenceId(queryParams));
        }


        [HttpGet("fromDate/{fromDate}/toDate/{toDate}/performanceType/{performanceTypeId}/pageSize/{pageSize}/pageNumber/{pageNumber}", Name = "GetAllByDatesAsync")]
        public async Task<IActionResult> GetAllByDatesAsync(ApprovalsQueryParams queryParams)
        {
            var approvals = await _approvalRepository.GetAllByDatesApprovalPerformanceAsync(queryParams, User.Identity.Name);
            Paginate(queryParams, approvals, "GetAllByDatesAsync");
            return Ok(approvals);
        }


        [HttpGet("fromDate/{fromDate}/toDate/{toDate}/approvalPurpose/{approvalPurposeId}/performanceType/{performanceTypeId}/pageSize/{pageSize}/pageNumber/{pageNumber}", Name = "GetAllByDatesApprovalPerformance")]
        public async Task<IActionResult> GetAllByDatesApprovalPerformance(ApprovalsQueryParams queryParams)
        {
            var approvals = await _approvalRepository.GetAllByDatesApprovalPerformanceV2Async(queryParams, User.Identity.Name);
            Paginate(queryParams, approvals, "GetAllByDatesApprovalPerformance");
            return Ok(approvals);
        }

        [HttpPut("{id}")]
        // public async Task<IActionResult> Update(string id, [FromBody] ApprovalCreatingDto budgetUpdatingDto)
        //{
        //    var loggedInUser = User.Identity.Name;
        //    var workForId = User.Claims.FirstOrDefault(c => c.Type == "WorkForId")?.Value;
        //    return Error.BadRequest("Sorry, valid forward to required.");

        //    ////budgetUpdatingDto.SetDefaultValue();
        //    //if (!budgetUpdatingDto.BudgetSub.Any())
        //    //    return Error.BadRequest("Sorry, no budget items found to save.");

        //    //if (budgetUpdatingDto.HasDuplicate())
        //    //    return Error.BadRequest("Sorry, duplicate budget items found.");

        //    //if (await _approvalRepository.IsApprovedAsync(budgetUpdatingDto.Id, _approvalPurposes.Budget, _performanceTypes.Approved))
        //    //    return Error.BadRequest($"Sorry, {budgetUpdatingDto.BudgetName} already approved. You cannot update.");

        //    //var loggedInUser = User.Identity.Name;
        //    //if (!await this._budgetRepository.IsValidPerson(loggedInUser, budgetUpdatingDto.Id))
        //    //    return Error.BadRequest("Sorry, you are not assigned person to this job.");

        //    var budgetToSave = Mapper.Map<ApprovalCreatingDto>(budgetUpdatingDto);

        //    budgetToSave.UpdatedBy = loggedInUser;
        //    budgetToSave.UpdatedOn = DateTime.UtcNow;

        //    _budgetRepository.UpdateBudget(budgetToSave);
        //    await _budgetRepository.CommitChangesAsync();
        //    return NoContent();
        //}

        [HttpGet("fromDate/{fromDate}/toDate/{toDate}/pageSize/{pageSize}/pageNumber/{pageNumber}", Name = "GetApprovals")]
        public async Task<IActionResult> GetApprovals(ApprovalsQueryParams queryParams)
        {
            var approvals = await _approvalRepository.GetAllByDatesAsync(queryParams, User.Identity.Name);
            Paginate(queryParams, approvals, "GetApprovals");
            return Ok(approvals);
        }

        private void Paginate(ApprovalsQueryParams queryParams, PagedList<ApprovalProcessViewModel> approvals, string actionName)
        {
            var paginationMetadata = new SqlPaginator<ResourceQueryParameters, ApprovalProcessViewModel>(_urlHelper)
                .GetPaginationMetadata(actionName, queryParams, approvals);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
        }
    }
}