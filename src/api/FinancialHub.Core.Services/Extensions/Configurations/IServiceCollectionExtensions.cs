using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Services.Mappers;
using FinancialHub.Core.Services.Services;

namespace FinancialHub.Core.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FinancialHubAutoMapperProfile));
            services.AddScoped<IMapperWrapper, FinancialHubMapperWrapper>();

            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IBalancesService, BalancesService>();

            services.AddScoped<IAccountBalanceService, AccountBalanceService>();
            services.AddScoped<ITransactionBalanceService, TransactionBalanceService>();
            
            return services;
        }
    }
}
