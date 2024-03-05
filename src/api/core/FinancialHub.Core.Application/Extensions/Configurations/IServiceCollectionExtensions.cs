using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Application.Services;
using FluentValidation;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Validators.Accounts;
using FinancialHub.Core.Application.Validators.Balances;
using FinancialHub.Core.Application.Validators.Categories;
using FinancialHub.Core.Application.Validators.Transactions;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services
                .AddCategoriesService()
                .AddAccountService()
                .AddBalanceService()
                .AddTransactionService();

            return services;
        }

        private static IServiceCollection AddBalanceService(this IServiceCollection services)
        {
            services.AddScoped<IBalancesService, BalancesService>();

            services.AddAutoMapper(typeof(BalanceMapper));

            services.AddScoped<IValidator<CreateBalanceDto>, CreateBalanceValidator>();
            services.AddScoped<IValidator<UpdateBalanceDto>, UpdateBalanceValidator>();

            return services;
        }

        private static IServiceCollection AddTransactionService(this IServiceCollection services)
        {
            services.AddScoped<ITransactionsService, TransactionsService>();

            services.AddAutoMapper(typeof(TransactionMapper));

            services.AddScoped<IValidator<CreateTransactionDto>, CreateTransactionValidator>();
            services.AddScoped<IValidator<UpdateTransactionDto>, UpdateTransactionValidator>();

            return services;
        }

        private static IServiceCollection AddAccountService(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService, AccountsService>();

            services.AddAutoMapper(typeof(AccountMapper));

            services.AddScoped<IAccountValidator, AccountValidator>();
            services.AddScoped<IValidator<CreateAccountDto>, CreateAccountValidator>();
            services.AddScoped<IValidator<UpdateAccountDto>, UpdateAccountValidator>();

            return services;
        }

        private static IServiceCollection AddCategoriesService(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddAutoMapper(typeof(CategoryMapper));

            services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryValidator>();
            services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryValidator>();

            return services;
        }
    }
}
