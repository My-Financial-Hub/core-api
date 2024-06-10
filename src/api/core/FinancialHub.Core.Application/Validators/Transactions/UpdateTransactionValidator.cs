using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Transactions
{
    internal class UpdateTransactionValidator : AbstractValidator<UpdateTransactionDto>
    {
        public UpdateTransactionValidator(IValidationErrorMessageProvider errorMessageProvider)
        {
            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.BalanceId)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required);

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required);

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
