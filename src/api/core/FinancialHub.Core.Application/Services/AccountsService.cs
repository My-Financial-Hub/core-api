using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Resources;
using Microsoft.Extensions.Logging;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsProvider provider;
        private readonly IAccountsValidator accountValidator;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AccountsService(
            IAccountsProvider provider, IAccountsValidator accountValidator,
            IMapper mapper, ILogger logger
        )
        {
            this.provider = provider;
            this.accountValidator = accountValidator;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto)
        {
            var validationResult = await this.accountValidator.ValidateAsync(accountDto);
            if(validationResult.HasError)
            {
                return validationResult.Error;
            }

            var account = this.mapper.Map<AccountModel>(accountDto);

            var result = await this.provider.CreateAsync(account);

            return this.mapper.Map<AccountDto>(result);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.provider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<AccountDto>>> GetAllAsync()
        {
            var accounts = await this.provider.GetAllAsync();

            return this.mapper.Map<ICollection<AccountDto>>(accounts).ToArray();
        }

        public async Task<ServiceResult<AccountDto>> GetByIdAsync(Guid id)
        {
            var accountExists = await this.accountValidator.ExistsAsync(id);
            if (accountExists.HasError)
            {
                return accountExists.Error;
            }

            var existingAccount = await this.provider.GetByIdAsync(id);

            return this.mapper.Map<AccountDto>(existingAccount);
        }

        public async Task<ServiceResult<AccountDto>> UpdateAsync(Guid id, UpdateAccountDto account)
        {
            var validationResult = await this.accountValidator.ValidateAsync(account);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            validationResult = await this.accountValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }
            
            var accountModel = this.mapper.Map<AccountModel>(account);

            var updatedAccount = await this.provider.UpdateAsync(id, accountModel);

            return this.mapper.Map<AccountDto>(updatedAccount);
        }
    }
}
