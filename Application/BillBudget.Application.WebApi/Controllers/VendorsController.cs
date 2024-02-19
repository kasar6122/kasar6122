using System.Threading.Tasks;
using BillBudget.Accounts.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/vendors")]
    public class AccountCodesController : Controller
    {
        private readonly IVendorRepository _vendorRepository;

        public AccountCodesController(IVendorRepository vendorRepository)
        {
            this._vendorRepository = vendorRepository;
        }


        [HttpGet("{term}")]
        public async Task<IActionResult> GetVendorsByName(string term) => Ok(await _vendorRepository.GetVendorsByNameAsync(term));

        [HttpGet]
        public async Task<IActionResult> GetVendorsAsync() =>
            Ok(await _vendorRepository.GetVendorsAsync());

    }
}