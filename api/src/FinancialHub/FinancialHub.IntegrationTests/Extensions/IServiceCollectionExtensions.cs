using FinancialHub.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace FinancialHub.IntegrationTests.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddTestDbContext<Context>(this IServiceCollection services)
            where Context : DbContext
        {
            var dbContextOptions = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<Context>)
            );
            services.Remove(dbContextOptions!);

            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(DbConnectionStringManager.ConnectionString);
                options.EnableSensitiveDataLogging(true);
                //TODO: remove it
                //https://stackoverflow.com/questions/482827/database-data-needed-in-integration-tests-created-by-api-calls-or-using-importe
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            return services;
        }
    }
}
