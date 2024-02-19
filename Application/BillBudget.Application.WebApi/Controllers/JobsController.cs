using System;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Dtos.QueryDtos;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Domain.Helpers;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.ViewModels;
using DaffodilSoftware.Core.SharedKernel;
using DaffodilSoftware.Pagination.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/jobs")]
    public class JobsController : Controller
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly JobStatusOptions _jobStatus;

        public JobsController(IJobRepository jobRepository,
            IUrlHelper urlHelper,
            IOptions<JobStatusOptions> jobStatus)
        {
            _jobRepository = jobRepository;
            _urlHelper = urlHelper;
            _jobStatus = jobStatus.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] JobCreatingDto jobCreating)
        {
            if (await _jobRepository.IsUniqueName(jobCreating.JobName))
                return Error.BadRequest($"Sorry, {jobCreating.JobName} already exist. Try another name");
            jobCreating.Status = _jobStatus.InProgress;

            var jobToSave = Mapper.Map<Job>(jobCreating);

            jobToSave.CreatedBy = User.Identity.Name;
            await _jobRepository.InsertNewAsync(jobToSave);
            await _jobRepository.CommitChangesAsync();

            var jobToView = Mapper.Map<JobUpdatingDto>(jobToSave);

            return CreatedAtAction("GetJob", new { id = jobToSave.Id }, jobToView);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Modify(string id, [FromBody] JobUpdatingDto jobUpdateDto)
        {
            if (await _jobRepository.IsUniqueName(jobUpdateDto.Id, jobUpdateDto.JobName))
                return Error.BadRequest($"Sorry, {jobUpdateDto.JobName} already exist. Try another Job name");

            jobUpdateDto.Status = _jobStatus.InProgress;

            var jobToSave = Mapper.Map<Job>(jobUpdateDto);

            jobToSave.UpdatedBy = User.Identity.Name;
            jobToSave.UpdatedOn = DateTime.UtcNow;

            _jobRepository.Modify(jobToSave);
            await _jobRepository.CommitChangesAsync();
            return NoContent();
        }


        [HttpGet("{id}", Name = "GetJob")]
        public async Task<IActionResult> GetJob(string id) =>
            Ok(await _jobRepository.GetById(id));


        [HttpGet("name/{term}")]
        public async Task<IActionResult> GetAutoJobs(JobQueryParams queryParameters) =>
            Ok(await _jobRepository.GetForAutoComplete(queryParameters.Term));


        [HttpGet("assignedTo/{assignTo}/costCenter/{costCenterId}/status/{status}/pageNumber/{pageNumber}/pageSize/{pageSize}", Name = "GetByAssingToCostCenterStatus")]
        public async Task<IActionResult> GetByAssingToCostCenterStatus(JobQueryParams queryParameters)
        {
            var jobs = await _jobRepository.GetAllByAssingToAsync(queryParameters, queryParameters.AssignTo, queryParameters.CostCenterId, queryParameters.Status);
            return Paginate(queryParameters, jobs, "GetByAssingToCostCenterStatus");
        }


        [HttpGet("assignedTo/{assignTo}/status/{status}/pageNumber/{pageNumber}/pageSize/{pageSize}", Name = "GetByAssingToStatus")]
        public async Task<IActionResult> GetByAssingToStatus(JobQueryParams queryParameters)
        {
            var jobs = await _jobRepository.GetAllByAssingToAsync(queryParameters, queryParameters.AssignTo, queryParameters.Status);
            return Paginate(queryParameters, jobs, "GetByAssingToStatus");
        }

        [HttpGet("costCenter/{costCenterId}/status/{status}/pageNumber/{pageNumber}/pageSize/{pageSize}", Name = "GetAllByCostCenterAsync")]
        public async Task<IActionResult> GetAllByCostCenterAsync(JobQueryParams queryParameters)
        {
            var jobs = await _jobRepository.GetAllByCostCenterAsync(queryParameters, queryParameters.Status, queryParameters.CostCenterId);
            return Paginate(queryParameters, jobs, "GetAllByCostCenterAsync");
        }


        [HttpGet("pageNumber/{pageNumber}/pageSize/{pageSize}", Name = "GetJobs")]
        public async Task<IActionResult> GetJobs(JobQueryParams queryParameters)
        {
            PagedList<JobGistViewModel> jobs;
            if (!User.IsInRole("admin"))
                jobs = await _jobRepository.GetAllByAssingToAsync(queryParameters, User.Identity.Name);
            else
                jobs = await _jobRepository.GetAllAsync(queryParameters);

            return Paginate(queryParameters, jobs, "GetJobs");
        }

        private IActionResult Paginate(JobQueryParams queryParameters, PagedList<JobGistViewModel> jobs, string actionName)
        {
            var metadata = new SqlPaginator<ResourceQueryParameters, JobGistViewModel>(_urlHelper)
                .GetPaginationMetadata(actionName, queryParameters, jobs);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(jobs);
        }
    }
}