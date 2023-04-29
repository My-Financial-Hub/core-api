using FluentValidation;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Resources;

namespace FinancialHub.Auth.Services.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator(IErrorMessageProvider provider)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(provider.Required)
                .EmailAddress()
                .WithMessage(provider.Invalid)
                .MaximumLength(300)
                .WithMessage(provider.MaxLength);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(provider.Required)
                .MaximumLength(300)
                .WithMessage(provider.MaxLength);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(provider.Required)
                .MaximumLength(300)
                .WithMessage(provider.MaxLength);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage(provider.Required);
        }
    }
}
