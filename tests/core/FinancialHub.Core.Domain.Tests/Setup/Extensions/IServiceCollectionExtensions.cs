using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Core.Domain.Tests.Builders.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Domain.Tests.Setup.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddModelBuilders(this IServiceCollection services)
        {
            services.AddTransient<AccountModelBuilder>();
            services.AddTransient<BalanceModelBuilder>();
            services.AddTransient<CategoryModelBuilder>();
            services.AddTransient<TransactionModelBuilder>();
            return services;
        }

        public static IServiceCollection AddEntityBuilders(this IServiceCollection services)
        {
            services.AddTransient<AccountEntityBuilder>();
            services.AddTransient<BalanceEntityBuilder>();
            services.AddTransient<CategoryEntityBuilder>();
            services.AddTransient<TransactionEntityBuilder>();
            return services;
        }
    }
}
