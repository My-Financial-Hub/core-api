using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Validators.Accounts
{
    internal class AccountsValidator : IAccountsValidator
    {
        private readonly IAccountsProvider accountsProvider;
        private readonly IValidator<CreateAccountDto> createValidator;
        private readonly IValidator<UpdateAccountDto> updateAccountDto;
        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly ILogger<AccountsValidator> logger;

        public AccountsValidator(
            IAccountsProvider accountsProvider, 
            IValidator<CreateAccountDto> createValidator,IValidator<UpdateAccountDto> updateAccountDto,
            IErrorMessageProvider errorMessageProvider, ILogger<AccountsValidator> logger
        )
        {
            this.accountsProvider = accountsProvider;
            this.createValidator = createValidator;
            this.updateAccountDto = updateAccountDto;
            this.errorMessageProvider = errorMessageProvider;
            this.logger = logger;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            this.logger.LogInformation("Validating the existence of account {id}", id);
            var result = await this.accountsProvider.GetByIdAsync(id);

            if (result != null)
            {
                this.logger.LogInformation("Account with {id} exists", id);
                return ServiceResult.Success;
            }

            var errorMessage = this.errorMessageProvider.NotFoundMessage("Account", id);
            this.logger.LogWarning("Validation error : {message}", errorMessage);

            return new NotFoundError(errorMessage);
        }

        public async Task<ServiceResult> ValidateAsync(CreateAccountDto createAccountDto)
        {
            this.logger.LogInformation("Validating Create account data");
            var result = await this.createValidator.ValidateAsync(createAccountDto);

            if (result.IsValid)
            {
                this.logger.LogInformation("Create account data validated");
                return ServiceResult.Success;
            }

            var errorMessage = errorMessageProvider.ValidationMessage("Account");
            this.logger.LogWarning("Validation error : {message}", errorMessage);

            var validationError = new ValidationError(
                message: errorMessage, 
                errors: result.Errors.ToFieldValidationError()
            );
            this.logger.LogTrace("Validation field errors : {errors}", validationError.Errors);
            return validationError;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateAccountDto updateAccountDto)
        {
            this.logger.LogInformation("Validating Update account data");
            var result = await this.updateAccountDto.ValidateAsync(updateAccountDto);

            if (result.IsValid)
            {
                this.logger.LogInformation("Update account data validated");
                return ServiceResult.Success;
            }

            var errorMessage = errorMessageProvider.ValidationMessage("Account");
            this.logger.LogWarning("Validation error : {message}", errorMessage);

            var validationError = new ValidationError(
                message: errorMessage,
                errors: result.Errors.ToFieldValidationError()
            );
            this.logger.LogTrace("Validation field errors : {errors}", validationError.Errors);
            return validationError;
        }
    }
}
