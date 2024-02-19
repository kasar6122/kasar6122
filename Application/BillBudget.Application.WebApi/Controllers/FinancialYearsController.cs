using System.Threading.Tasks;
using BillBudget.Accounts.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize]
    [Route("api/FinancialYears")]
    public class FinancialYearsController : Controller
    {
        private readonly IFinancialYearsRepository _projectsRepository;
        public FinancialYearsController(IFinancialYearsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentFinancialYear() => Ok(await _projectsRepository.GetCurrentFinancialYear());
    }
}