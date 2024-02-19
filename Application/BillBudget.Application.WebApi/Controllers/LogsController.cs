using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/Logs")]
    public class LogsController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public LogsController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        [HttpGet("{date}")]
        public async Task<IActionResult> GetLogByDate(string date)
        {
            var fileStream = new FileStream($"{_environment.WebRootPath}/Logs/Serilog-{date}.txt", FileMode.Open);
            using (var reader = new StreamReader(fileStream))
                return Ok(await reader.ReadToEndAsync());
        }

    }
}