using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Auth.Domain.Interfaces.Helpers;
using FinancialHub.Auth.Infra.Mappers;
using FinancialHub.Auth.Infra.Providers;
using FinancialHub.Auth.Infra.Helpers;

namespace FinancialHub.Auth.Infra.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserProvider, UserProvider>();

            services.AddScoped<ICredentialProvider, CredentialProvider>();
            services.AddScoped<ISignupProvider, SignupProvider>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();

            services.AddSingleton(provider => 
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new FinancialHubAuthProviderProfile());
                    using var scope = provider.CreateScope();
                    cfg.AddProfile(new FinancialHubAuthCredentialProfile(scope.ServiceProvider.GetService<IPasswordHelper>()!));
                }
            ).CreateMapper());

            return services;
        }
    }
}
