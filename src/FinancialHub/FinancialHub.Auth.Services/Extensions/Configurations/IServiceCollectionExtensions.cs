using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Mappers;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Configurations;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection("TokenServiceSettings").Get<TokenServiceSettings>();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    options =>
                    {
                        var key = Encoding.ASCII.GetBytes(settings.SecurityKey);

                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new()
                        {
                            //ValidAudience = settings.Audience,
                            ValidateAudience = false,

                            //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256Signature },
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuerSigningKey = true,

                            //ValidIssuer = settings.Issuer,
                            ValidateIssuer = false,

                            //ValidateLifetime = true,
                            //RequireExpirationTime = true,
                            //ClockSkew = TimeSpan.FromMinutes(60),
                        };
                    }
                );
            return services;
        }

        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProfile));

            services.AddSettings(configuration);
            services.AddAuthConfiguration(configuration);
            services.AddServices();

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
            services.Configure<TokenServiceSettings>(configuration.GetSection("TokenServiceSettings"));
            return services;
        }
    }
}
