using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/performaceTypes")]
    public class PerformaceTypesController : Controller
    {
        private readonly IPerformaceTypeRepository _performaceTypeRepository;

        public PerformaceTypesController(IPerformaceTypeRepository performaceTypeRepository)
        {
            _performaceTypeRepository = performaceTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _performaceTypeRepository.GetAllPerformTypesAsync());

    }
}