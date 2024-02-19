using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/unadjustedJobs")]
    public class UnadjustedJobsController : Controller
    {
        private readonly IJobRepository _jobRepository;

        public UnadjustedJobsController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }


        [HttpGet("assignedTo/{assignedTo}")]
        public async Task<IActionResult> GetUnadjustedJobsSummaryByAssignedTo(string assignedTo)
            => Ok(await _jobRepository.GetUnadjustedJobsSummaryByAssignedToAsync(assignedTo));


        [HttpGet]
        public async Task<IActionResult> GetJobSummary() =>
             Ok(await _jobRepository.GetJobSummaryAsync());
    }
}