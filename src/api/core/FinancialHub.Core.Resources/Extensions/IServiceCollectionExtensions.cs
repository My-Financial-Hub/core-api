using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Providers;

namespace FinancialHub.Core.Resources.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthResources(this IServiceCollection services)
        {
            services.AddSingleton<IValidationErrorMessageProvider>(
                new ValidationErrorMessageProvider(CultureInfo.CurrentCulture)
            );
            return services;
        }
    }
}
