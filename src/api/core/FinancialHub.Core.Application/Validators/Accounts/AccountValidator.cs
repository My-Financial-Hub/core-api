﻿using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Accounts
{
    public class AccountValidator : IAccountValidator
    {
        private readonly IValidator<CreateAccountDto> createValidator;
        private readonly IValidator<UpdateAccountDto> updateAccountDto;
        private readonly IErrorMessageProvider errorMessageProvider;

        public AccountValidator(
            IValidator<CreateAccountDto> createValidator,IValidator<UpdateAccountDto> updateAccountDto,
            IErrorMessageProvider errorMessageProvider
        )
        {
            this.createValidator = createValidator;
            this.updateAccountDto = updateAccountDto;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult> ValidateAsync(CreateAccountDto createAccountDto)
        {
            var result = await this.createValidator.ValidateAsync(createAccountDto);

            if (result.IsValid)
            {
                return ServiceResult.Success;
            }
            
            return new ValidationError(
                message: errorMessageProvider.ValidationMessage("Account"), 
                errors: result.Errors.ToFieldValidationError()
            );
        }

        public async Task<ServiceResult> ValidateAsync(UpdateAccountDto updateAccountDto)
        {
            var result = await this.updateAccountDto.ValidateAsync(updateAccountDto);

            if (result.IsValid)
            {
                return ServiceResult.Success;
            }

            return new ValidationError(
                message: errorMessageProvider.ValidationMessage("Account"),
                errors: result.Errors.ToFieldValidationError()
            );
        }
    }
}
