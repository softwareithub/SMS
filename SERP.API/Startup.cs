using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SERP.Infrastructure.Implementation.Infratructure.Implementation;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using Swashbuckle.AspNetCore.Swagger;

namespace SERP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddMvc();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "SERP API ",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),

                });
            });
            var key = Encoding.ASCII.GetBytes("SERPKEY");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericImplementation<,>));
            services.AddTransient<ITimeSheetRepo, TimeSheetImplementation>();
            services.AddTransient<ISubjectMasterRepo, SubjectImplementation>();
            services.AddTransient<ITimeSheetRepo, TimeSheetImplementation>();
            services.AddTransient<IFeeDetailRepo, FeeDetailImplementation>();
            services.AddTransient<IDashBoardGraphRepo, DashBoardRepo>();
            services.AddTransient<IFeeDepositRecieptRepo, FeeDepositRepo>();
            services.AddTransient<IBookDetailReport, BookDetailReportImplementation>();
            services.AddTransient<ISalaryHeadRepo, SalarySlipImplementation>();
            services.AddTransient<IAccountTransactionRepo, AccountTransactionImplementation>();
            services.AddTransient<IOnlineTestSubmitRepository, OnlineTestSubmitImplementation>();
            services.AddTransient<IQuickSearchRepo, QuickSearchImplementation>();
            services.AddTransient<ILessonRepository, LessonImplmentation>();
            services.AddTransient<IReportRepository, ReportImplementation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "SERP.API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
