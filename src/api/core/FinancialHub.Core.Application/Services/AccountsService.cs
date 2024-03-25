using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;
using Microsoft.Extensions.Logging;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FinancialHub.Common.Extensions;

namespace FinancialHub.Core.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsProvider provider;
        private readonly IAccountsValidator accountValidator;
        private readonly IMapper mapper;
        private readonly ILogger<AccountsService> logger;

        public AccountsService(
            IAccountsProvider provider, IAccountsValidator accountValidator,
            IMapper mapper, ILogger<AccountsService> logger
        )
        {
            this.provider = provider;
            this.accountValidator = accountValidator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto)
        {
            this.logger.LogInformation("Creating account {name}", accountDto.Name);
            this.logger.LogTrace("Account data : {accountDto}", accountDto.ToJson());
            
            var validationResult = await this.accountValidator.ValidateAsync(accountDto);
            if(validationResult.HasError)
            {
                this.logger.LogTrace("Account creation Validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed creating account {name}", accountDto.Name);
                return validationResult.Error;
            }

            var account = this.mapper.Map<AccountModel>(accountDto);

            var accountModel = await this.provider.CreateAsync(account);

            var result = this.mapper.Map<AccountDto>(accountModel);

            this.logger.LogTrace("Account creation result : {result}", result.ToJson());
            this.logger.LogInformation("Account {name} Sucessfully created", result.Name);
            
            return result;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing account {id}", id);
            var amount = await this.provider.DeleteAsync(id);
            this.logger.LogInformation("Account {id} {removed}", id, amount > 0 ? "removed" : "not removed");
            return amount;
        }

        public async Task<ServiceResult<ICollection<AccountDto>>> GetAllAsync()
        {
            this.logger.LogInformation("Getting all accounts");
            var accounts = await this.provider.GetAllAsync();

            this.logger.LogInformation("Returning {count} accounts", accounts.Count > 0? $"{accounts.Count}": "no");
            return this.mapper.Map<ICollection<AccountDto>>(accounts).ToArray();
        }

        public async Task<ServiceResult<AccountDto>> GetByIdAsync(Guid id)
        {
            this.logger.LogInformation("Getting account {id}", id);

            var validationResult = await this.accountValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Account get by id result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed getting account {id}", id);
                return validationResult.Error;
            }

            var existingAccount = await this.provider.GetByIdAsync(id);

            var account = this.mapper.Map<AccountDto>(existingAccount);

            this.logger.LogTrace("Account result {account}", account.ToJson());
            this.logger.LogInformation("Account {id} found", id);
            return account;
        }

        public async Task<ServiceResult<AccountDto>> UpdateAsync(Guid id, UpdateAccountDto account)
        {
            this.logger.LogInformation("Updating account {id}", id);
           
            var validationResult = await this.accountValidator.ValidateAsync(account);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Account update validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed updating account {id}", id);
                return validationResult.Error;
            }

            validationResult = await this.accountValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Account update validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed updating account {id}", id);
                return validationResult.Error;
            }
            
            var accountModel = this.mapper.Map<AccountModel>(account);

            var updatedAccount = await this.provider.UpdateAsync(id, accountModel);

            var result = this.mapper.Map<AccountDto>(updatedAccount);

            this.logger.LogTrace("Account update result : {result}", result.ToJson());
            this.logger.LogInformation("Account {id} Sucessfully Updated", id);

            return result;
        }
    }
}
