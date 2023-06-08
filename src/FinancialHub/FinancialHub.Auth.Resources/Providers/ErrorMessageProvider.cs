using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Resources.Resources.Errors;
using System.Globalization;

namespace FinancialHub.Auth.Resources.Providers
{
    public class ErrorMessageProvider : IErrorMessageProvider
    {
        private readonly CultureInfo cultureInfo;

        public ErrorMessageProvider(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public string? Required  => ErrorMessages.ResourceManager.GetString("Required", this.cultureInfo);
        public string? MaxLength => ErrorMessages.ResourceManager.GetString("MaxLength", this.cultureInfo);
        public string? Invalid   => ErrorMessages.ResourceManager.GetString("Invalid", this.cultureInfo);
    }
}
