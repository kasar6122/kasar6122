using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkForController : ControllerBase
    {
        private readonly IWorkForRepository _workForRepository;

        public WorkForController(IWorkForRepository workForRepository)
        {
            _workForRepository = workForRepository;
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> Get(string employeeId)
            => Ok(await _workForRepository.GetWorkForIdsAsync(employeeId));
    }
}