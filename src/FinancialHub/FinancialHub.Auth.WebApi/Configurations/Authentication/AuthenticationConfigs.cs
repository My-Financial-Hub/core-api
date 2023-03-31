using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinancialHub.Auth.WebApi.Configurations.Authentication
{
    public static class AuthenticationConfigs
    {
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    "user",
                    options => 
                    {
                        var jwtSettings = configuration.GetSection("JwtSettings");
                        var key = Encoding.ASCII.GetBytes(jwtSettings["SecurityKey"]);
                        options.Authority = jwtSettings["Authority"];
                        options.Audience = jwtSettings["Audience"];
                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuer = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),

                            ValidIssuer = jwtSettings["Issuer"],

                            ValidateLifetime = true,
                            RequireExpirationTime = true,
                            ClockSkew = TimeSpan.FromMinutes(60),
                        };
                    }
                );
            return services;
        }
    }
}
