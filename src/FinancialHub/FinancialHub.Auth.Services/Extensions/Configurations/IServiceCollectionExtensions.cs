using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddAuthValidators();
            return services;
        }

        private static IServiceCollection AddAuthValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<UserModel>, UserValidator>();
            return services;
        }
    }
}
