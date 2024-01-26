using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Resources.Errors.Validation;
using System.Globalization;

namespace FinancialHub.Core.Resources.Providers
{
    public class ValidationErrorMessageProvider : IValidationErrorMessageProvider
    {
        private readonly CultureInfo cultureInfo;

        public ValidationErrorMessageProvider(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public string? Required         => ValidationErrorMessages.ResourceManager.GetString(nameof(Required), this.cultureInfo);

        public string? ExceedMaxLength  => ValidationErrorMessages.ResourceManager.GetString(nameof(ExceedMaxLength), this.cultureInfo);

        public string? GreaterThan      => ValidationErrorMessages.ResourceManager.GetString(nameof(GreaterThan), this.cultureInfo);

        public string? OutOfEnum        => ValidationErrorMessages.ResourceManager.GetString(nameof(OutOfEnum), this.cultureInfo);
    }
}
