using FinancialHub.Auth.Services.Configurations;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinancialHub.Auth.Services.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettings(configuration);

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IValidator<SigninModel>, SigninModelValidator>();
            services.AddScoped<IValidator<SignupModel>, SignupModelValidator>();

            services.AddScoped<ISigninService, SigninService>();
            services.AddScoped<ISignupService, SignupService>();

            services.AddAuthConfiguration(configuration);

            return services;
        }

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

        private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenServiceSettings>(configuration.GetSection("TokenServiceSettings"));
            return services;
        }
    }
}
