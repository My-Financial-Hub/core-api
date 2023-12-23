using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Infra.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreInfra(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FinancialHubAutoMapperProfile));

            services.AddScoped<ICategoriesProvider, CategoriesProvider>();
            services.AddScoped<IAccountsProvider, AccountsProvider>();
            services.AddScoped<IBalancesProvider, BalancesProvider>();
            services.AddScoped<ITransactionsProvider, TransactionsProvider>();

            return services;
        }
    }
}
