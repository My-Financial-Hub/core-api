using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Auth.Services.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddAuthentication(configuration);

            services.AddAuthValidators();
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
