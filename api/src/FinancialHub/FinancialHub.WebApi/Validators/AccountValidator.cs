using FinancialHub.Domain.Models;
using FinancialHub.Domain.Resources;
using FinancialHub.WebApi.Resources;
using FluentValidation;

namespace FinancialHub.WebApi.Validators
{
    public class AccountValidator : AbstractValidator<AccountModel>
    {
        private static string CustomMessage(string errorMessage, params object[] values)
        {
            var valuesSize = values?.Length ?? 0;
            var args = new object[valuesSize+1];
            args[0] = FinancialHubConcepts.Account;
            values.CopyTo(args, 1);

            return string.Format(errorMessage, args);
        }

        public AccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(CustomMessage(ErrorMessages.Required, "Name"))
                .Length(0,50)
                .WithMessage(CustomMessage(ErrorMessages.ExceedMaxLength, "Name", 50));

            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(CustomMessage(ErrorMessages.ExceedMaxLength, "Description", 500));

            RuleFor(x => x.Currency)
                .Length(0,50)
                .WithMessage(CustomMessage(ErrorMessages.ExceedMaxLength, "Currency", 50));
        }
    }
}
