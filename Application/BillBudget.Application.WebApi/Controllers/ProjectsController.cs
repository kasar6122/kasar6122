using System.Threading.Tasks;
using BillBudget.Accounts.Logic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepository _projectsRepository;
        public ProjectsController(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet("{term}")]
        public async Task<IActionResult> GetByNames(string term) => Ok(await _projectsRepository.GetByNameAsync(term));
    }
}