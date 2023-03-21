using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Data.Contexts;
using FinancialHub.Infra.Data.Repositories;

namespace FinancialHub.Infra.Data.Extensions.Configurations
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
