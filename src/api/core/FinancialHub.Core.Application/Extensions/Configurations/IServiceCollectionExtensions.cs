using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Application.Validators;
using FluentValidation;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Application.Validators.Accounts;
using FinancialHub.Core.Application.Mappers;

namespace FinancialHub.Core.Application.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddServices();
            services.AddValidators();
            services.AddAutoMapper(typeof(FinancialHubAccountMapper));

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IBalancesService, BalancesService>();

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AccountModel>, AccountValidator>();
            services.AddScoped<IValidator<CategoryModel>, CategoryValidator>();
            services.AddScoped<IValidator<TransactionModel>, TransactionValidator>();

            services.AddScoped<IValidator<CreateAccountDto>, CreateAccountValidator>();

            return services;
        }
    }
}
