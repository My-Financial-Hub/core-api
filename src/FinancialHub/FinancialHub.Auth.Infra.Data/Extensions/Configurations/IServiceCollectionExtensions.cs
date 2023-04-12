using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using FinancialHub.Auth.Infra.Data.Repositories;

namespace FinancialHub.Auth.Infra.Data.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        private static IServiceCollection AddAuthDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FinancialHubAuthContext>(
                provider =>
                    provider.UseSqlServer(
                        configuration.GetConnectionString("auth"),
                        x => x
                            .MigrationsAssembly("FinancialHub.Auth.Infra.Migrations")
                            .MigrationsHistoryTable("auth-migrations")
                    )
            );

            return services;
        }

        public static IServiceCollection AddAuthRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthDatabase(configuration);
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
