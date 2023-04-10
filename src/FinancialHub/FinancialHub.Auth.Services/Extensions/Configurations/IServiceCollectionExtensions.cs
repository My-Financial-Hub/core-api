using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Mappers;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetValue<TokenServiceSettings>("TokenServiceSettings");
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

        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProfile));

            services.AddSettings(configuration);
            services.AddServices();
            services.AddAuthConfiguration(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.BindSettings<TokenServiceSettings>(configuration, "TokenServiceSettings");
            return services;
        }

        private static IServiceCollection BindSettings<T>(this IServiceCollection services, IConfiguration configuration, string section) where T: class
        {
            return services.Configure<T>(configuration.GetSection(section)); 
        }
    }
}
