using System.IO;
using System.Text;
using System.Threading.Tasks;
using BillBudget.Application.WebApi.Dtos;
using BillBudget.Application.WebApi.Extentions;
using BillBudget.Application.WebApi.Helpers;
using BillBudget.Core.Domain.Dtos;
using BillBudget.Core.Domain.Helpers;
using DaffodilSoftware.ActivityLogging;
using DaffodilSoftware.Core.SharedKernel;
using eCure.Accessibility.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace BillBudget.Application.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMemoryCache();

            services.AddSingleton(_configuration);

            //services.Configure<EmailOptions>(option => _configuration.GetSection("EmailOptions").Bind(option));

            services.Configure<OtherOptions>(option => _configuration.GetSection("OtherOptions").Bind(option));

            services.Configure<ApprovalPurposes>(option => _configuration.GetSection("ApprovalPurposes").Bind(option));

            services.Configure<BudgetPerformTypes>(option => _configuration.GetSection("BudgetPerformTypes").Bind(option));

            services.Configure<ConnectionStringsSection>(option => _configuration.GetSection("ConnectionStrings").Bind(option));

            services.Configure<UserFromDiuAuth>(option => _configuration.GetSection("UserFromDiuAuth").Bind(option));

            services.Configure<ApplicationStatus>(option => _configuration.GetSection("ApplicationStatus").Bind(option));

            services.Configure<TableViewMapping>(option => _configuration.GetSection("TableViewMapping").Bind(option));

            services.Configure<VoucherPrefix>(option => _configuration.GetSection("VoucherPrefix").Bind(option));

            services.Configure<FinancialTransactionTypes>(option => _configuration.GetSection("FinancialTransactionType").Bind(option));

            services.Configure<BillTypes>(option => _configuration.GetSection("BillTypes").Bind(option));

            services.Configure<JobStatusOptions>(option => _configuration.GetSection("JobStatusOptions").Bind(option));


            services.AddOptions();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        // .WithOrigins("http://192.168.10.37,http://192.168.10.37/app")
                        
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            // services.ConfigureJwt(_configuration);
            var jwt = JwtConfigurator.GetJwt(_configuration.GetConnectionString("JwtDb"));

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = jwt.Audience,
                ValidIssuer = jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey))
            };

            services.AddAuthentication(jwt.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = validationParameters;
                });

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });

            services
               .AddMvc(options =>
               {
                  // options.Filters.Add(typeof(ActivityLogFilter));
                   options.Filters.Add(typeof(AuthorizationFromSqlFilter));
                   options.Filters.Add(typeof(ValidateModelStateFilter));
               })
               .AddXmlSerializerFormatters();

            services.AddRouting();

            services.ResolveDependencies(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                 {
                     appBuilder.Run(async context =>
                     {
                         context.Response.StatusCode = 500;
                         await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                     });
                 });

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo
                    .RollingFile(Path.Combine(env.WebRootPath + "/Logs", "Serilog-{Date}.txt"))
                    .CreateLogger();
            }

            app.Use(async (context, next) =>
            {
                await Task.Run(() => context.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination,Authorization"));
                await next();
            });

            app.UseCors("AllowAll");
            app.UseMvcWithDefaultRoute();
            app.ConfigureMapper();
        }
    }
}
