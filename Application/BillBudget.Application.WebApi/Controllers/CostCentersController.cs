using System;
using System.Threading.Tasks;
using AutoMapper;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Dtos.QueryDtos;
using BillBudget.Core.Domain.Entities;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.ViewModels;
using DaffodilSoftware.Core.SharedKernel;
using DaffodilSoftware.Pagination.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/costCenters")]
    public class CostCentersController : Controller
    {
        private readonly ICostCenterRepository _centerRepository;
        private readonly IUrlHelper _urlHelper;

        public CostCentersController(ICostCenterRepository centerRepository, IUrlHelper urlHelper)
        {
            _centerRepository = centerRepository;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CostCenterCreatingDto costCenter)
        {
            if (await _centerRepository.IsUniqueName(costCenter.CostCenterName))
                return Error.BadRequest($"Sorry, {costCenter.CostCenterName} already exist. Try another name");

            var costCenterToSave = Mapper.Map<CostCenter>(costCenter);
            costCenterToSave.CreatedBy = User.Identity.Name;

            await _centerRepository.InsertNewAsync(costCenterToSave);
            await _centerRepository.CommitChangesAsync();

            var costCenterView = Mapper.Map<CostCenterViewModel>(costCenterToSave);

            return CreatedAtAction("GetCostCenter", new { id = costCenterView.Id }, costCenterView);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CostCenterUpdatingDto costCenter)
        {
            if (await _centerRepository.IsUniqueName(id, costCenter.CostCenterName))
                return Error.BadRequest($"Sorry, {costCenter.CostCenterName} already exist. Try another name");

            var costCenterToUpdate = Mapper.Map<CostCenter>(costCenter);
            costCenterToUpdate.CreatedBy = User.Identity.Name;
            costCenterToUpdate.CreatedOn = DateTime.UtcNow;

            _centerRepository.Modify(costCenterToUpdate);
            await _centerRepository.CommitChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetCostCenter")]
        public async Task<IActionResult> GetCostCenter(string id) => Ok(await _centerRepository.GetById(id));

        [HttpGet("name/{term}")]
        public async Task<IActionResult> GetForAutoComplete(CostCenterQueryParams queryParameters) =>
            Ok(await _centerRepository.GetForAutoComplete(queryParameters.Term));

        [HttpGet("status/{status}/pageSize/{pageSize:int}/pageNumber/{pageNumber:int}")]
        public async Task<IActionResult> GetCostCenterByStatusWithPagination(CostCenterQueryParams queryParameters)
        {
            var costCenters = await _centerRepository.GetAllAsync(queryParameters, queryParameters.Status ?? false);
            var metadata = new SqlPaginator<ResourceQueryParameters, CostCenterViewModel>(_urlHelper)
                .GetPaginationMetadata("GetCostCenters", queryParameters, costCenters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(costCenters);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetCostCenterByStatus(CostCenterQueryParams queryParameters) =>
            Ok(await _centerRepository.GetAllAsync(queryParameters, queryParameters.Status ?? false));

        [HttpGet(Name = "GetCostCenters")]
        public async Task<IActionResult> GetCostCenter(CostCenterQueryParams queryParameters)
        {
            var costCenters = await _centerRepository.GetAllAsync(queryParameters);
            var metadata = new SqlPaginator<ResourceQueryParameters, CostCenterViewModel>(_urlHelper)
                .GetPaginationMetadata("GetCostCenters", queryParameters, costCenters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(costCenters);
        }
    }
}