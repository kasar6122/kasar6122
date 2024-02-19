using System.Collections.Generic;
using System.Threading.Tasks;
using BillBudget.Application.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BillBudget.Application.WebApi.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await Task.CompletedTask;
            return Ok(new List<object>()
            {

            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] BudgetSubCreatingDto obj)
        {
            return Ok();
        }



        [HttpGet("{date}")]
        public async Task<IActionResult> Get(string date)
        {

            var val = "";
            var cok = Request.Cookies.TryGetValue("cookieKey", out val);
            Response.HttpContext.Response.Cookies.Append("cookieKey", "cookieVal", new CookieOptions()
            {

            });

            await Task.CompletedTask;
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
