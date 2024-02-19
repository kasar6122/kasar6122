using System.Linq;
using System.Threading.Tasks;
using BillBudget.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly BillBudgetCoreContext _context;

        public TestController(BillBudgetCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _context.Approval
                .GroupBy(a => new { a.Ref_Id, a.JobId })
                .Where(a => a.Any(g => g.BudgetPerformTypeId == "1" && g.BudgetPerformTypeId == "1"))
                .ToListAsync();
            return Ok(data);
        }
    }
}