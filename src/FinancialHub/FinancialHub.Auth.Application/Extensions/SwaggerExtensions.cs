using System.Reflection;
using System.Reflection.Metadata;
using FinancialHub.Auth.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

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
