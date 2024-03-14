using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Resources;
using FinancialHub.Core.Resources.Resources.Errors;
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
            var message = ErrorMessages.ResourceManager.GetString("NotFound", this.cultureInfo) ?? string.Empty;
            var concept = ConceptWords.ResourceManager.GetString(name, this.cultureInfo) ?? name;
            return string.Format(message, concept, id);
        }

        public string UpdateFailedMessage(string name, Guid id)
        {
            var message = ErrorMessages.ResourceManager.GetString("UpdateFailed", this.cultureInfo) ?? string.Empty;
            var concept = ConceptWords.ResourceManager.GetString(name, this.cultureInfo) ?? name;
            return string.Format(message, concept, id);
        }

        public string ValidationMessage(string name)
        {
            var concept = ConceptWords.ResourceManager.GetString(name, this.cultureInfo) ?? name;
            var message = ErrorMessages.ResourceManager.GetString("InvalidData", this.cultureInfo) ?? string.Empty;
            return string.Format(message, concept);
        }
    }
}
