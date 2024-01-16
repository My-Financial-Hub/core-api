using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Application.Validators;
using FluentValidation;

namespace FinancialHub.Core.Application.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddMapper();
            services.AddServices();
            services.AddValidators();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IBalancesService, BalancesService>();

            services.AddScoped<ITransactionBalanceService, TransactionBalanceService>();

            return services;
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddScoped<IMapperWrapper, FinancialHubMapperWrapper>();

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AccountModel>, AccountValidator>();
            services.AddScoped<IValidator<CategoryModel>, CategoryValidator>();
            services.AddScoped<IValidator<TransactionModel>, TransactionValidator>();

            return services;
        }
    }
}
