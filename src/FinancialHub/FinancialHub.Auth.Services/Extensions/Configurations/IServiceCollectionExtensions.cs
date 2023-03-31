using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Mappers;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Services.Configurations;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProfile));

            services.AddSettings(configuration);
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
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
