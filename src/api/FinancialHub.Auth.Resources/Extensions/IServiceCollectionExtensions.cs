using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace FinancialHub.Auth.Resources.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthResources(this IServiceCollection services)
        {
            services.AddSingleton<IErrorMessageProvider>(
                new ErrorMessageProvider(CultureInfo.CurrentCulture)
            );
            return services;
        }
    }
}
