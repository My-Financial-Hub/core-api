using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Services.Validators.Rules;
using FluentValidation;

namespace FinancialHub.Auth.Services.Validators
{
    public class SigninModelValidator : AbstractValidator<SigninModel>
    {
        public SigninModelValidator(IErrorMessageProvider provider)
        {
            RuleFor(x => x.Email)
                .ValidEmail(provider);

            RuleFor(x => x.Password)
                .ValidPassword(provider);
        }
    }
}
