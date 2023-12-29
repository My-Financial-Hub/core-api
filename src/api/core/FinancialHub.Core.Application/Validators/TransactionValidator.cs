using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionModel>
    {
        public TransactionValidator(IValidationErrorMessageProvider errorMessageProvider)
        {
            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(errorMessageProvider.GreaterThan);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(errorMessageProvider.OutOfEnum);

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage(errorMessageProvider.OutOfEnum);
        }
    }
}
