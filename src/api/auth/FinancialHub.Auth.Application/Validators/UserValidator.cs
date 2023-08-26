using FluentValidation;
using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Application.Validators.Rules;

namespace FinancialHub.Auth.Application.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator(IErrorMessageProvider provider)
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
        }
    }
}
