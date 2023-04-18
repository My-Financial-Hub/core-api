using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Infra.Providers;
using FinancialHub.Auth.Infra.Mappers;

namespace FinancialHub.Auth.Infra.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProviderProfile));
            services.AddScoped<IUserProvider, UserProvider>();
            return services;
        }
    }
}
