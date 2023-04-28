using FluentValidation;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Services.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is invalid")
                .MaximumLength(300)
                .WithMessage("Email exceeds the max length of 300");

            RuleFor(x => x.FirstName)
                .MaximumLength(300)
                .WithMessage("FirstName exceeds the max length of 300")
                .NotEmpty()
                .WithMessage("FirstName is required");

            RuleFor(x => x.LastName)
                .MaximumLength(300)
                .WithMessage("LastName exceeds the max length of 300")
                .NotEmpty()
                .WithMessage("LastName is required");

            RuleFor(x => x.BirthDate)
                .NotNull()
                .WithMessage("LastName exceeds the max length of 300");
        }
    }
}
