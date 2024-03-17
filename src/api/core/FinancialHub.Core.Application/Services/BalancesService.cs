using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IBalancesProvider balancesProvider;
        private readonly IBalancesValidator balancesValidator;
        private readonly IAccountsValidator accountsValidator;
        private readonly ILogger<BalancesService> logger;
        private readonly IMapper mapper;

        public BalancesService(
            IBalancesProvider balancesProvider,
            IBalancesValidator balancesValidator, IAccountsValidator accountsValidator,
            IMapper mapper, ILogger<BalancesService> logger
        )
        {
            this.balancesProvider = balancesProvider;
            this.balancesValidator = balancesValidator;
            this.accountsValidator = accountsValidator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResult<BalanceDto>> CreateAsync(CreateBalanceDto balance)
        {
            this.logger.LogInformation("Creating balance {name} in account {id}", balance.Name, balance.AccountId);
            this.logger.LogTrace("Balance data : {balance}", balance);

            var validationResult = await this.balancesValidator.ValidateAsync(balance);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance creation Validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed creating account {name}", balance.Name);
                return validationResult.Error;
            }

            validationResult = await this.accountsValidator.ExistsAsync(balance.AccountId);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance creation Validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Account {AccountId} not found", balance.AccountId);
                return validationResult.Error;
            }

            var balanceModel = this.mapper.Map<BalanceModel>(balance);

            var createdBalance = await this.balancesProvider.CreateAsync(balanceModel);

            var result = this.mapper.Map<BalanceDto>(createdBalance);
            this.logger.LogTrace("Balance creation result : {result}", result);
            this.logger.LogInformation("Balance {name} Sucessfully created in account {id}", result.Name, result.Account.Id);
            return result;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing balance {id}", id);
            var amount = await this.balancesProvider.DeleteAsync(id);
            this.logger.LogInformation("Balance {id} {removed}", id, amount > 0 ? "removed" : "not removed");
            return amount;
        }

        public async Task<ServiceResult<BalanceDto>> GetByIdAsync(Guid id)
        {
            this.logger.LogInformation("Getting balance {id}", id);
            var validationResult = await this.balancesValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance get by id result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed getting balance {id}", id);
                return validationResult.Error;
            }

            var existingBalance = await this.balancesProvider.GetByIdAsync(id);

            var balance = this.mapper.Map<BalanceDto>(existingBalance);

            this.logger.LogTrace("Balance result {balance}", balance);
            this.logger.LogInformation("Balance {id} found", id);
            return balance;
        }

        public async Task<ServiceResult<ICollection<BalanceDto>>> GetAllByAccountAsync(Guid accountId)
        {
            this.logger.LogInformation("Getting balances from account {accountId}", accountId);
            var validationResult = await this.accountsValidator.ExistsAsync(accountId);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balances get by account result : {validationResult}", validationResult);
                this.logger.LogInformation("Account {AccountId} not found", accountId);
                return validationResult.Error;
            }

            var balances = await this.balancesProvider.GetAllByAccountAsync(accountId);

            var result = this.mapper.Map<ICollection<BalanceDto>>(balances).ToArray();

            this.logger.LogTrace("Balance result {balances}", balances);
            this.logger.LogInformation(
                "{amount} returned from account {accountId}", 
                result.Length >0 ? $"{result.Length} balances" : "no balances",
                accountId
            );
            
            return result;
        }

        public async Task<ServiceResult<BalanceDto>> UpdateAsync(Guid id, UpdateBalanceDto balance)
        {
            this.logger.LogInformation("Updating balance {name} in account {id}", balance.Name, balance.AccountId);
            this.logger.LogTrace("Balance data : {balance}", balance);
            var validationResult = await this.balancesValidator.ValidateAsync(balance);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance update validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed updating balance {id}", id);
                return validationResult.Error;
            }

            validationResult = await this.balancesValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance update validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Balance {id} not found", id);
                return validationResult.Error;
            }

            validationResult = await this.accountsValidator.ExistsAsync(balance.AccountId);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance update validation result : {validationResult}", validationResult);
                this.logger.LogInformation("Account {AccountId} not found", balance.AccountId);
                return validationResult.Error;
            }

            var balanceModel = this.mapper.Map<BalanceModel>(balance);

            var updatedBalance = await this.balancesProvider.UpdateAsync(id, balanceModel);

            var result = this.mapper.Map<BalanceDto>(updatedBalance);
            this.logger.LogTrace("Balance update result : {result}", result);
            this.logger.LogInformation("Balance {name} Sucessfully created", result.Name);
            return result;
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            this.logger.LogInformation("Getting balance {id}", id);
            var validationResult = await this.balancesValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Balance get by id result : {validationResult}", validationResult);
                this.logger.LogInformation("Failed getting balance {id}", id);
                return validationResult.Error;
            }

            var updatedBalance = await balancesProvider.UpdateAmountAsync(id, newAmount);

            this.logger.LogTrace("Update Balance amount result : {updatedBalance}", updatedBalance);
            this.logger.LogInformation("Update Balance amount {id} in account {id}", updatedBalance.Id, updatedBalance.AccountId);
            return updatedBalance;
        }
    }
}
