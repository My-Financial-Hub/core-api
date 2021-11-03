using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Infra.Repositories;
using FinancialHub.Infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.WebApi.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConfigurations(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            return services;
        }
        //TODO: find a better method organization
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            return services;
        }
    }
}
