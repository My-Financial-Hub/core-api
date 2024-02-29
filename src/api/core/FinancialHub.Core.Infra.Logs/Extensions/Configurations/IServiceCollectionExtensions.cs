using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Infra.Logs.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreLogging(this IServiceCollection services)
        {
            services.AddLogging();
            return services;
        }
    }
}
