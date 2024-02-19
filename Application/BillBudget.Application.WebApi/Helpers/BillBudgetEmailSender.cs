using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.ViewModels;
using Microsoft.Extensions.Options;
using RabbitMq.Core;
using RabbitMq.Core.Abstractions;
using Microsoft.AspNetCore.Hosting;

namespace BillBudget.Application.WebApi.Helpers
{
    public class BillBudgetEmailSender : IBillBudgetEmailSender
    {
        private readonly IEmployeeRepository _employeeData;
        private readonly IHostingEnvironment _environment;
        private readonly RabbitMq.Core.EmailOptions _emailOptions;
        private readonly RabbitConnectionOption _rabbitMqConnection;
        private readonly IWorkerProducer _rabbitMqWorkerProducer;
        private readonly ClaimsPrincipal _claimsPrincipal;
        public BillBudgetEmailSender(
            IEmployeeRepository employeeData,
            IOptions<RabbitConnectionOption> rabbitMqConnection,
            IWorkerProducer rabbitMqWorkerProducer,
            IOptions<RabbitMq.Core.EmailOptions> emailOptions,
            IHostingEnvironment environment,
            IPrincipal principal)
        {
            _employeeData = employeeData;
            _environment = environment;
            _emailOptions = emailOptions.Value;
            _rabbitMqConnection = rabbitMqConnection.Value;
            _rabbitMqWorkerProducer = rabbitMqWorkerProducer;
            _claimsPrincipal = principal as ClaimsPrincipal;
        }

        private string GetValueFromToken(string claimType)
        => _claimsPrincipal.Claims.FirstOrDefault(u => u.Type == claimType)?.Value;

        public async Task SendEmailAsync(string forwardTo, string url, string subject)
        {
            var reportTo = GetValueFromToken("SupervisorId");

            EmployeeViewModel employee = null;
            if (!string.IsNullOrEmpty(forwardTo))
                employee = await _employeeData.GetEmployeeByIdAsync(forwardTo);
            else if (!string.IsNullOrEmpty(reportTo))
                employee = await _employeeData.GetEmployeeByIdAsync(reportTo);

            if (employee != null)
            {

                var description = $"<br/> {GetValueFromToken("GivenName")}" +
                                  $"<br/>{GetValueFromToken("Designation")}" +
                                  $"<br/>{GetValueFromToken("Department")}" +
                                  $"<br/>{_claimsPrincipal.Identity.Name}";

                _emailOptions.ReceipientName = employee.Name;
                _emailOptions.To = employee.Email;
                _emailOptions.Message = PopulateEmailBody("Bill-Budget", url, description);
                _emailOptions.Subject = subject;
                _rabbitMqWorkerProducer.Produce(_rabbitMqConnection, _emailOptions.Serialize());
            }
        }

        private string PopulateEmailBody(string title, string url, string description)
        {
            var webRoot = _environment.WebRootPath;
            var htmlFile = Path.Combine(webRoot, "BudgetAssignedNotificationEmailTemplate.html");
            string body;
            using (var reader = new StreamReader(htmlFile))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ReceipientName}", _emailOptions.ReceipientName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{Description}", description);
            return body;
        }
    }
}
