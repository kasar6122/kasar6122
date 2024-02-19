using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/measurementUnits")]
    public class MeasurementUnitController : Controller
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;

        public MeasurementUnitController(IMeasurementUnitRepository measurementUnitRepository)
        {
            _measurementUnitRepository = measurementUnitRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
            => Ok(await _measurementUnitRepository.GetAllMeasurementUnitsAsync());
    }
}