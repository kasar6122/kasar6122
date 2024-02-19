using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BillBudget.Application.WebApi.Helpers
{
    public interface IBillBudgetEmailSender
    {
        //Task SendEmailAsync(string forwardTo, string url, string subject, IEnumerable<Claim> claims);
        Task SendEmailAsync(string forwardTo, string url, string subject);
    }
}
