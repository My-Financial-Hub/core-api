using FluentValidation;
using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Services.Validators.Rules;

namespace FinancialHub.Auth.Services.Validators
{
    public class SignupModelValidator : AbstractValidator<SignupModel>
    {
        public SignupModelValidator(IErrorMessageProvider provider)
        {
            RuleFor(x => x.Email)
                .ValidEmail(provider);

            RuleFor(x => x.FirstName)
                .ValidName(provider);

            RuleFor(x => x.LastName)
                .ValidName(provider);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage(provider.Required);

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage(provider.Required)
                .MinimumLength(8)
                .WithMessage(provider.MinLength)
                .MaximumLength(80)
                .WithMessage(provider.MaxLength);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(provider.ConfirmPassword);
        }
    }
}
