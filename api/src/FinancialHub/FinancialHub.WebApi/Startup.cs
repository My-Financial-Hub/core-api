using FinancialHub.Infra.Data.Contexts;
using FinancialHub.WebApi.Extensions.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FinancialHub.WebApi
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
            services.AddDbContext<FinancialHubContext>(
                provider =>{ 
                    provider.UseSqlServer(
                        Configuration.GetConnectionString("default"),
                        x => x
                            .MigrationsAssembly("FinancialHub.Infra.Migrations")
                            .MigrationsHistoryTable("migrations")
                    );
                }
            );

            //services.AddLogging();

            services.AddRepositories();
            services.AddServices();
            services.AddValidators();
            services.AddApiConfigurations();

            //services.AddHealthCheck();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financial Hub WebApi", Version = "v1" });
            });

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

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
