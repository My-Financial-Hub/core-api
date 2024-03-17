using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using FinancialHub.Core.Infra.Logs.Extensions.Configurations;

namespace FinancialHub.Core.WebApi.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConfigurations(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            return services;
        }

        public static IServiceCollection AddApiDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financial Hub WebApi", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddApiLogging(this IServiceCollection services)
        {
            services.AddCoreLogging();
            services
                .AddHttpLogging(logging =>
                {
                    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                });
            return services;
        }
    }
}
