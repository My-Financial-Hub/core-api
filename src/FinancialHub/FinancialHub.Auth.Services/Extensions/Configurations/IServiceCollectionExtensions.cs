using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Mappers;
using FinancialHub.Auth.Services.Services;

namespace FinancialHub.Auth.Services.Extensions.Configurations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FinancialHubAuthProfile));
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
