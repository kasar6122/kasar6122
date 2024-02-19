using System.Linq;
using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using DaffodilSoftware.Core.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Authorize(Roles = "accounts")]
    [Route("api/jobDone")]
    public class JobDoneController : Controller
    {
        private readonly IJobDoneRepository _jobDoneRepository;
        private readonly IFinancialTransactionRepository _transactionRepository;

        public JobDoneController(IJobDoneRepository jobDoneRepository,
            IFinancialTransactionRepository transactionRepository)
        {
            _jobDoneRepository = jobDoneRepository;
            _transactionRepository = transactionRepository;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobStatus(string id)
        {
            var allTransactions = await _transactionRepository.GetByJobIdAsync(id);
            if (allTransactions.Any())
            {
                var summation = allTransactions.First();
                if (summation.GrandTotalDr != summation.GrandTotalCr)
                    return Error.BadRequest("Sorry, debit and credit sides are not equal.");

                await _jobDoneRepository.JobDoneAsync(id, User.Identity.Name);
                await _jobDoneRepository.CommitChangesAsync();
                return NoContent();
            }
            return Error.BadRequest("Sorry, debit and credit sides are not equal.");
        }



    }
}