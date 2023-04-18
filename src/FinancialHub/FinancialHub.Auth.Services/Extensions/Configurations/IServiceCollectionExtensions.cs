using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Mappers;
using FinancialHub.Auth.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProfile));
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
