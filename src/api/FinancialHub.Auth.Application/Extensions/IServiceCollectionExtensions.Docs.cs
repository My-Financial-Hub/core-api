using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FinancialHub.Auth.Application.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        public static IEnumerable<OpenApiSecurityScheme> GetAuthSecuritySchemes(this IServiceCollection _)
        {
            return new[]
            {
                new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                }
            };
        }
    }
}
