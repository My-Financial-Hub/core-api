using FluentValidation;
using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Application.Validators.Rules;

namespace FinancialHub.Auth.Application.Validators
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
                .NotNull()
                .WithMessage(provider.Required);

            RuleFor(x => x.Password)
                .ValidPassword(provider);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(provider.ConfirmPassword);
        }
    }
}
