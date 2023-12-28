using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Resources.Errors.Validation;
using System.Globalization;

namespace FinancialHub.Core.Resources.Providers
{
    public class ErrorMessageProvider : IErrorMessageProvider
    {
        private readonly CultureInfo cultureInfo;

        public ErrorMessageProvider(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public string NotFoundMessage(string name, Guid id)
        {
            var message = ValidationErrorMessages.ResourceManager.GetString("NotFound", this.cultureInfo) ?? string.Empty;
            return string.Format(message, name, id);
        }

        public string UpdateFailedMessage(string name, Guid id)
        {
            var message = ValidationErrorMessages.ResourceManager.GetString("UpdateFailed", this.cultureInfo) ?? string.Empty;
            return string.Format(message, name, id);
        }
    }
}
