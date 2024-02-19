using System;
using System.Linq;
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
    [Route("api/workOrders")]
    public class WorkOrdersController : Controller
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IJobRepository _jobRepository;

        public WorkOrdersController(IWorkOrderRepository workOrderRepository, IJobRepository jobRepository)
        {
            this._workOrderRepository = workOrderRepository;
            _jobRepository = jobRepository;
        }

        [HttpGet("{id}", Name = "GetWorkOrderById")]
        public async Task<IActionResult> GetWorkOrderById(string id) => Ok(await _workOrderRepository.GetWorkOrderByIdAsync(id));

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetWorkOrderByJobIdAsync(string jobId) => Ok(await _workOrderRepository.GetWorkOrderByJobIdAsync(jobId));

        [HttpGet("keyvalue/job/{jobId}")]
        public async Task<IActionResult> GetWorkOrderKeyValueByJobIdAsync(string jobId) => Ok(await _workOrderRepository.GetWorkOrderKeyValueByJobIdAsync(jobId));

        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] WorkOrderMainCreatingDto workOrder)
        {
            if (!await _jobRepository.IsThisJobAssignedToThisUser(workOrder.JobId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person.");
            
            if (await _workOrderRepository.IsUniqueWorkOrderNumberAsync(workOrder.WorkOrderNumber))
                return Error.BadRequest($"Sorry, work order number {workOrder.WorkOrderNumber} already exist.");

            workOrder.SetDefaultValue();

            if (workOrder.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate  work order items items found.");

            var workOrderToSave = Mapper.Map<WorkOrderMain>(workOrder);
            workOrderToSave.WorkOrderDate = DateTime.UtcNow;

            workOrderToSave.CreatedBy = User.Identity.Name;
            await _workOrderRepository.InsertNewAsync(workOrderToSave);
            await _workOrderRepository.CommitChangesAsync();

            var jobToView = Mapper.Map<WorkOrderMainUpdatingDto>(workOrderToSave);

            return CreatedAtAction("GetWorkOrderById", new { id = workOrderToSave.Id }, jobToView);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] WorkOrderMainUpdatingDto workOrder)
        {
            if (!await _jobRepository.IsThisJobAssignedToThisUser(workOrder.JobId, User.Identity.Name))
                return Error.BadRequest($"Sorry, you are not valid person.");

            if (await _workOrderRepository.IsUniqueWorkOrderNumberAsync(workOrder.Id, workOrder.WorkOrderNumber))
                return Error.BadRequest($"Sorry, work order number {workOrder.WorkOrderNumber} already exist.");

            workOrder.SetDefaultValue();

            if (!workOrder.WorkOrderSub.Any())
                return Error.BadRequest("Sorry, no work order items found to save.");

            if (workOrder.HasDuplicate())
                return Error.BadRequest("Sorry, duplicate  work order items items found.");

            var workOrderToSave = Mapper.Map<WorkOrderMain>(workOrder);

            workOrderToSave.CreatedBy = User.Identity.Name;
            _workOrderRepository.Modify(workOrderToSave);
            await _workOrderRepository.CommitChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _workOrderRepository.DeleteWorkOrder(id);
            await _workOrderRepository.CommitChangesAsync();
            return NoContent();
        }

        [HttpDelete("sub/{id}")]
        public async Task<IActionResult> DeleteSubAsync(string id)
        {
            await _workOrderRepository.DeleteWorkOrderSub(id);
            await _workOrderRepository.CommitChangesAsync();
            return NoContent();
        }
    }
}