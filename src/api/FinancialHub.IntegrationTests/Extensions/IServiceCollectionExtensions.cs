using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            return services;
        }
    }
}
