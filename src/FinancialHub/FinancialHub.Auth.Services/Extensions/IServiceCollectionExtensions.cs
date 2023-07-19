using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FinancialHub.Auth.Services.Extensions
{
    [ExcludeFromCodeCoverage]
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthServices();

            services.AddAuthentication(configuration);

            services.AddAuthValidators();

            return services;
        }

        private static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        private static IServiceCollection AddAuthValidators(this IServiceCollection services)
        {
            services.AddFluentValidation(x =>
            {
                x.AutomaticValidationEnabled = true;
                x.DisableDataAnnotationsValidation = true;
                x.ValidatorOptions.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            });
            services.AddScoped<IValidator<UserModel>, UserValidator>();

            return services;
        }
    }
}
