using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Infra.Caching.Repositories;
using FinancialHub.Core.Infra.Caching.Configurations;

namespace FinancialHub.Core.Infra.Caching.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCachingConfiguration(configuration)
                .AddRedisCaching(configuration)
                .AddCachingServices();

            return services;
        }

        private static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = configuration.GetConnectionString("cache");
            });

            return services;
        }

        private static IServiceCollection AddCachingServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsCache, AccountsCache>();
            services.AddScoped<IBalancesCache, BalancesCache>();
            services.AddScoped<ICategoriesCache, CategoriesCache>();
            return services;
        }

        private static IServiceCollection AddCachingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CacheConfiguration>(options =>
            {
                options.ExpirationTime =  int.Parse(configuration["Cache:ExpirationTime"]);
            });
            return services;
        }
    }
}
