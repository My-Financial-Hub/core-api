using FinancialHub.Auth.Domain.Models;
using FluentValidation;

namespace FinancialHub.Auth.Services.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {

        }
    }
}
