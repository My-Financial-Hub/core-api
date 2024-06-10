using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Validators.Balances
{
    internal class BalancesValidator : IBalancesValidator
    {
        private readonly IBalancesProvider balancesProvider;
        private readonly IAccountsValidator accountValidator;
        private readonly IValidator<CreateBalanceDto> createBalanceDtoValidator;
        private readonly IValidator<UpdateBalanceDto> updateBalanceDtoValidator;

        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly ILogger<BalancesValidator> logger;

        public BalancesValidator(
            IBalancesProvider balancesProvider, 
            IAccountsValidator accountValidator,
            IValidator<CreateBalanceDto> createBalanceDtoValidator, IValidator<UpdateBalanceDto> updateBalanceDtoValidator,
            IErrorMessageProvider errorMessageProvider, ILogger<BalancesValidator> logger
        )
        {
            this.balancesProvider = balancesProvider;
            this.accountValidator = accountValidator;
            this.createBalanceDtoValidator = createBalanceDtoValidator;
            this.updateBalanceDtoValidator = updateBalanceDtoValidator;
            this.errorMessageProvider = errorMessageProvider;
            this.logger = logger;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            this.logger.LogInformation("Validating the existence of balance {id}", id);
            var result = await this.balancesProvider.GetByIdAsync(id);
            if (result != null)
            {
                this.logger.LogInformation("Balance with {id} exists", id);
                return ServiceResult.Success;
            }

            var errorMessage = this.errorMessageProvider.NotFoundMessage("Balance", id);
            this.logger.LogWarning("Validation error : {message}", errorMessage);
            return new NotFoundError(errorMessage);
        }

        public async Task<ServiceResult> ValidateAsync(CreateBalanceDto createBalanceDto)
        {
            this.logger.LogInformation("Validating Create Balance data");
            var accountExists = await this.accountValidator.ExistsAsync(createBalanceDto.AccountId);
            if (accountExists.HasError)
            {
                return accountExists.Error;
            }

            var result = await this.createBalanceDtoValidator.ValidateAsync(createBalanceDto);
            if (!result.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage("Balance");
                var fieldErrors = result.Errors.ToFieldValidationError();
                this.logger.LogWarning("Validation error : {message}", errorMessage);
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Create Balance data validated");
            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateBalanceDto updateBalanceDto)
        {
            this.logger.LogInformation("Validating Update Balance data");
            var accountExists = await this.accountValidator.ExistsAsync(updateBalanceDto.AccountId);
            if (accountExists.HasError) 
            {
                return accountExists.Error;
            }

            var balanceValid = await this.updateBalanceDtoValidator.ValidateAsync(updateBalanceDto);
            if (!balanceValid.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage("Balance");
                var fieldErrors = balanceValid.Errors.ToFieldValidationError();
                this.logger.LogWarning("Validation error : {message}", errorMessage);
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Update Balance data validated");
            return ServiceResult.Success;
        }
    }
}
