using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Application.Extensions;
using FinancialHub.Auth.Resources.Extensions;
using FinancialHub.Auth.Infra.Extensions;
using FinancialHub.Auth.Infra.Data.Extensions;

namespace FinancialHub.Auth.Presentation.Extensions
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
