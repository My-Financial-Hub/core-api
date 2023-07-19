using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Infra.Data.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace FinancialHub.Auth.Infra.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        private static IServiceCollection AddAuthDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FinancialHubAuthContext>(
                provider =>
                    provider.UseSqlServer(
                        configuration.GetConnectionString("auth"),
                        x => x.MigrationsHistoryTable("auth_migrations")
                    )
            );

            return services;
        }

        public static IServiceCollection AddAuthRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthDatabase(configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();

            return services;
        }
    }
}
