using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Balances
{
    public class UpdateBalanceValidator : AbstractValidator<UpdateBalanceDto>
    {
        public UpdateBalanceValidator(IValidationErrorMessageProvider errorMessageProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required)
                .Length(0, 200)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.Currency)
                .Length(0, 50)
                .WithMessage(errorMessageProvider.ExceedMaxLength); 

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required);
        }
    }
}
