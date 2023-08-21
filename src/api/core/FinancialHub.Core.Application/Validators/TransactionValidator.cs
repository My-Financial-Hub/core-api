using FinancialHub.Core.Application.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionModel>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(ErrorMessages.GreaterThan);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(ErrorMessages.OutOfEnum);

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage(ErrorMessages.OutOfEnum);
        }
    }
}
