using FinancialHub.Core.Services.Resources;
using FluentValidation;

namespace FinancialHub.Core.Services.Validators
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
