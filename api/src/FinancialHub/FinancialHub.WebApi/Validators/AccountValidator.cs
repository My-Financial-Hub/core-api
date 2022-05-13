using FinancialHub.Domain.Models;
using FinancialHub.WebApi.Resources;
using FluentValidation;

namespace FinancialHub.WebApi.Validators
{
    public class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ErrorMessages.Required)
                .Length(0,200)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.Currency)
                .Length(0,50)
                .WithMessage(ErrorMessages.ExceedMaxLength);
        }
    }
}
