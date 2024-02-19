using System.Threading.Tasks;
using BillBudget.Accounts.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Areas.Accounts.Controllers
{
    [Authorize]
    [Route("api/accountHeads")]
    public class PostableAccountHeadsController : Controller
    {
        private readonly IAccountHeadsRepository _accountCodeRepository;

        public PostableAccountHeadsController(IAccountHeadsRepository accountCodeRepository)
        {
            this._accountCodeRepository = accountCodeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHeads() => 
            Ok(await _accountCodeRepository.GetHeadsAsync());


        [HttpGet("{term}")]
        public async Task<IActionResult> GetVendorsByName(string term) => Ok(await _accountCodeRepository.GetAccountCodesNameAsync(term));


        [HttpGet("cash/{term}")]
        public async Task<IActionResult> GetCashHeadsAsync(string term) => Ok(await _accountCodeRepository.GetCashHeadsAsync(term));


        [HttpGet("bank/{term}")]
        public async Task<IActionResult> GetCashBankHeadsAsync(string term) => Ok(await _accountCodeRepository.GetCashBankHeadsAsync(term));
    }
}