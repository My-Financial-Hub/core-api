using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators
{
    public class BalanceValidator : AbstractValidator<BalanceModel>
    {
        public BalanceValidator(IValidationErrorMessageProvider errorMessageProvider) : base()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required)
                .Length(0, 200)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required);
        }
    }
}
