using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FinancialHub.Auth.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FinancialHub.Auth.Application.Extensions
{
    [ExcludeFromCodeCoverage]
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

                var schemes = services.GetAuthSecuritySchemes();

                var requeriments = new OpenApiSecurityRequirement();

                foreach (var scheme in schemes)
                {
                    c.AddSecurityDefinition(scheme.Reference.Id, scheme);

                    requeriments.Add(scheme, Array.Empty<string>());
                }

                c.AddSecurityRequirement(requeriments);
            });
            return services;
        }
    }
}
