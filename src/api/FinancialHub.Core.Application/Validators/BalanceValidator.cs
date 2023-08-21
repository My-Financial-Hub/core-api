using FinancialHub.Core.Application.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators
{
    public class BalanceValidator : AbstractValidator<BalanceModel>
    {
        public BalanceValidator() : base()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ErrorMessages.Required)
                .Length(0, 200)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage(ErrorMessages.Required);
        }
    }
}
