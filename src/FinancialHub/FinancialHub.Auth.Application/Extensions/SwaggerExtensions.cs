using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

namespace FinancialHub.Auth.Application.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddAuthDocs(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                    )
                );
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Financial Hub Auth Api",
                        Version = "v1"
                    }
                );
            });
            return services;
        }

        public static void A()
        {

        }
    }
}
