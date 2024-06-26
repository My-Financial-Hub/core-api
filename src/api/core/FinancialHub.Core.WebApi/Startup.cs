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
using FinancialHub.Core.WebApi.Middlewares;
using FinancialHub.Core.Infra.Caching.Extensions.Configurations;

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
            services
                .AddApiConfigurations()
                .AddApiDocs()
                .AddApiLogging();

            services.AddCoreResources()
                .AddCoreServices()
                .AddCoreInfra()
                .AddCaching(Configuration)
                .AddRepositories(Configuration);

            services
                .AddMvc()
                .AddNewtonsoftJson();
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
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
