using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Services.Extensions;
using FinancialHub.Auth.Resources.Extensions;
using FinancialHub.Auth.Infra.Extensions;
using FinancialHub.Auth.Infra.Data.Extensions;

namespace FinancialHub.Auth.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthServices(configuration)
                .AddAuthResources()
                .AddAuthProviders(configuration)
                .AddAuthRepositories(configuration);

            return services;
        }
    }
}
