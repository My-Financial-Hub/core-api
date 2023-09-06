using FinancialHub.Auth.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Auth.Application.Validators.Rules
{
    public static class ValidatorRulesExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidEmail<T>(
            this IRuleBuilderInitial<T, string> builder, IErrorMessageProvider provider
        )
        {
            return builder
                .NotEmpty()
                .WithMessage(provider.Required)
                .EmailAddress()
                .WithMessage(provider.Invalid)
                .MaximumLength(300)
                .WithMessage(provider.MaxLength);
        }

        public static IRuleBuilderOptions<T, string> ValidName<T>(
            this IRuleBuilderInitial<T, string> builder, IErrorMessageProvider provider
        )
        {
            return builder
                .NotEmpty()
                .WithMessage(provider.Required)
                .MaximumLength(300)
                .WithMessage(provider.MaxLength);
        }

        public static IRuleBuilderOptions<T, string> ValidPassword<T>(
            this IRuleBuilderInitial<T, string> builder, IErrorMessageProvider provider
        )
        {
            return builder
                .NotNull()
                .NotEmpty()
                .WithMessage(provider.Required)
                .MinimumLength(8)
                .WithMessage(provider.MinLength)
                .MaximumLength(80)
                .WithMessage(provider.MaxLength);
        }
    }
}
