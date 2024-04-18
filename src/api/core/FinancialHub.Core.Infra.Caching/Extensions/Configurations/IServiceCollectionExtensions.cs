using FinancialHub.Core.Infra.Caching.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Infra.Caching.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = configuration.GetConnectionString("cache");
            });
            services.AddScoped<IAccountsCache, AccountsCache>();
            services.AddScoped<IBalancesCache, BalancesCache>();
            services.AddScoped<ICategoriesCache, CategoriesCache>();
            return services;
        }
    }
}
