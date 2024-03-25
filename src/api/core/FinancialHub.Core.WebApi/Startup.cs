using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialHub.Core.WebApi.Extensions.Configurations;
using FinancialHub.Core.Application.Extensions.Configurations;
using FinancialHub.Core.Infra.Extensions;
using FinancialHub.Core.Resources.Extensions;
using FinancialHub.Core.Infra.Data.Extensions.Configurations;
using FinancialHub.Core.Infra.Logs.Extensions.Configurations;

namespace FinancialHub.Core.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfigurations();
            services.AddApiDocs();
            services.AddApiLogging();

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
            });

            services.AddCoreResources();
            services.AddCoreServices();
            services.AddCoreInfra();
            services.AddRepositories(Configuration);

            services.AddMvc().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial Hub WebApi v1"));
            }

            app.UseRouting();
            app.UseLogRequest();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
