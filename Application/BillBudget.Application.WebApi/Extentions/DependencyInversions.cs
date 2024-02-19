using System.Security.Principal;
using BillBudget.Accounts.Data;
using BillBudget.Accounts.Logic.Abstractions;
using BillBudget.Accounts.Logic.Concretes;
using BillBudget.Application.WebApi.Helpers;
using BillBudget.Core.Data;
using BillBudget.Core.Logic.Abstractions;
using BillBudget.Core.Logic.Concretes;
using BillBudget.Voucher.Data;
using DaffodilSoftware.Core.SharedKernel;
using eCure.Accessibility.NetCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Core;

namespace BillBudget.Application.WebApi.Extentions
{
    public static class DependencyInversions
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration conf)
        {
            services.AddScoped(typeof(BillBudgetVoucherContext));
            services.AddScoped(typeof(BillBudgetCoreContext));
            services.AddScoped(typeof(BillBudgetAccountsContext));
            services.AddScoped(typeof(eCure.Accessibility.NetCore.Data.AccessibilityDbContext));
            services.AddScoped<eCure.Accessibility.NetCore.Logic.Abstractions.IUserAccessibiltyRepository, eCure.Accessibility.NetCore.Logic.Implementations.UserAccessibiltyRepository>();
            //services.AddScoped<IUrlAccessibilityDataRepository, UrlAccessibilityDataRepository>();
            services.ConfigureRabbitConnection(conf);
            services.AddScoped<ICostCenterRepository, CostCenterRepository>();
            services.AddScoped<IWorkForRepository, WorkForRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobDoneRepository, JobDoneRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IBudgetSubRepository, BudgetSubRepository>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();
            //services.AddScoped<IEmployeeData, EmployeeDataRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISqlConnector, SqlConnector>();
            services.AddScoped<IAdvanceRepository, AdvanceRepository>();
            services.AddScoped<IAdvanceSubRepository, AdvanceSubRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IAccountHeadsRepository, AccountHeadRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillSubRepository, BillSubRepository>();
            services.AddScoped<IVoucherRepository, VoucherBuilderRepository>();
            services.AddScoped<IVoucherDataRepository, VoucherDataRepository>();
            services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
            services.AddScoped<IPerformaceTypeRepository, PerformaceTypeRepository>();
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IFinancialYearsRepository, FinancialYearsRepository>();
            services.AddScoped<ICalculatorRepository, CalculatorRepository>();
            services.AddScoped<IApprovalCommitteeRepository, ApprovalCommitteeRepository>();
            services.AddScoped<IApprovedAmountRepository, ApprovedAmountRepository>();
          //  services.AddScoped<IUrlAccessibilityDataRepository, UrlAccessibilityDataRepository>();
            services.AddScoped<IBillBudgetEmailSender, BillBudgetEmailSender>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });
        }
    }
}
