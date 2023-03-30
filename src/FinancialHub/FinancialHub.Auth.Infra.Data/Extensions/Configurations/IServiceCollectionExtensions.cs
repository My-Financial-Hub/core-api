using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;
using FinancialHub.Auth.Infra.Data.Repositories;

namespace FinancialHub.Auth.Infra.Data.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FinancialHubAuthContext>(
                provider =>
                    provider.UseSqlServer(
                        configuration.GetConnectionString("auth"),
                        x => x
                            .MigrationsAssembly("FinancialHub.Auth.Infra.Data.Migrations")
                            .MigrationsHistoryTable("auth-migrations")
                    )
            );
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
