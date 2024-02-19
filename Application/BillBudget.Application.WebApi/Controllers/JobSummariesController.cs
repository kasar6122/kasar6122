using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/jobSummaries")]
    public class JobSummariesController : Controller
    {
        private readonly IJobRepository _jobRepository;
        public JobSummariesController(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> CalculateTransactionSummary(string jobId) =>
            Ok(await _jobRepository.CalculateTransactionSummaryAsync(jobId));
    }
}