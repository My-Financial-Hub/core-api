using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Balances
{
    public class BalanceValidator : IBalancesValidator
    {
        private readonly IAccountsProvider accountsProvider;
        private readonly IBalancesProvider balancesProvider;
        private readonly IValidator<CreateBalanceDto> createBalanceDto;
        private readonly IValidator<UpdateBalanceDto> updateBalanceDto;
        private readonly IErrorMessageProvider errorMessageProvider;

        public BalanceValidator(
            IAccountsProvider accountsProvider, IBalancesProvider balancesProvider,
            IValidator<CreateBalanceDto> createBalanceDto, IValidator<UpdateBalanceDto> updateBalanceDto,
            IErrorMessageProvider errorMessageProvider
        )
        {
            this.balancesProvider = balancesProvider;
            this.accountsProvider = accountsProvider;
            this.createBalanceDto = createBalanceDto;
            this.updateBalanceDto = updateBalanceDto;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult> AccountExistsAsync(Guid accountId)
        {
            var result = await this.accountsProvider.GetByIdAsync(accountId);
            if(result != null)
            {
                return ServiceResult.Success;
            }

            return new NotFoundError(
                this.errorMessageProvider.NotFoundMessage("Account", accountId)
            );
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            var result = await this.balancesProvider.GetByIdAsync(id);
            if (result != null)
            {
                return ServiceResult.Success;
            }

            return new NotFoundError(
                this.errorMessageProvider.NotFoundMessage("Balance", id)
            );
        }

        public async Task<ServiceResult> ValidateAsync(CreateBalanceDto createBalanceDto)
        {
            var accountExists = await this.AccountExistsAsync(createBalanceDto.AccountId);
            if (accountExists.HasError)
            {
                return accountExists.Error;
            }

            var result = await this.createBalanceDto.ValidateAsync(createBalanceDto);
            if (!result.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage("Balance"),
                    errors: result.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateBalanceDto updateBalanceDto)
        {
            var accountExists = await this.AccountExistsAsync(updateBalanceDto.AccountId);
            if (accountExists.HasError) 
            {
                return accountExists.Error;
            }

            var balanceValid = await this.updateBalanceDto.ValidateAsync(updateBalanceDto);
            if (!balanceValid.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage("Balance"),
                    errors: balanceValid.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }
    }
}
