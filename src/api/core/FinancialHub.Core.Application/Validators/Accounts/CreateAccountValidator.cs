﻿using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Accounts
{
    internal class CreateAccountValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountValidator(IValidationErrorMessageProvider errorMessageProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required)
                .Length(0, 200)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(errorMessageProvider.ExceedMaxLength);
        }
    }
}
