using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Infra.Data.Contexts;
using FinancialHub.Core.Infra.Data.Repositories;

namespace FinancialHub.Core.Infra.Data.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FinancialHubContext>(
                provider =>
                    provider.UseSqlServer(
                        configuration.GetConnectionString("default"),
                        x => x
                            .MigrationsAssembly("FinancialHub.Infra.Migrations")
                            .MigrationsHistoryTable("migrations")
                    )
            );
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddScoped<IBalancesRepository, BalancesRepository>();

            return services;
        }
    }
}
