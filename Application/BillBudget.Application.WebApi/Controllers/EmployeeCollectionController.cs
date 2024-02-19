using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/v2/employees")]
    [ApiController]
    public class EmployeeCollectionController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeCollectionController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery]string term)
            => Ok(await _employeeRepository.GetEmployeeAsync(term));
    }
}