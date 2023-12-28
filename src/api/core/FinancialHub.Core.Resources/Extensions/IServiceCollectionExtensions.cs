using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Providers;

namespace FinancialHub.Core.Resources.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreResources(this IServiceCollection services)
        {
            var cultureInfo = CultureInfo.CurrentCulture;

            services.AddSingleton<IValidationErrorMessageProvider>(
                new ValidationErrorMessageProvider(cultureInfo)
            );
            services.AddSingleton<IErrorMessageProvider>(
                new ErrorMessageProvider(cultureInfo)
            );
            return services;
        }
    }
}
