using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsProvider provider;
        private readonly IMapper mapper;
        private readonly IErrorMessageProvider errorMessageProvider;

        [Obsolete("Remove it later")]
        public AccountsService(IAccountsProvider provider, IErrorMessageProvider errorMessageProvider)
        {
            this.provider = provider;
            this.errorMessageProvider = errorMessageProvider;
        }

        public AccountsService(IAccountsProvider provider, IMapper mapper,IErrorMessageProvider errorMessageProvider)
        {
            this.provider = provider;
            this.mapper = mapper;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account)
        {
            return await this.provider.CreateAsync(account);
        }

        public async Task<ServiceResult<AccountDto>> CreateAsync(CreateAccountDto accountDto)
        {
            var account = this.mapper.Map<AccountModel>(accountDto);

            var result = await this.provider.CreateAsync(account);

            return this.mapper.Map<AccountDto>(result);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.provider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<AccountModel>>> GetAllByUserAsync(string userId)
        {
            var accounts = await this.provider.GetAllAsync();

            return accounts.ToArray();
        }

        public async Task<ServiceResult<AccountModel>> GetByIdAsync(Guid id)
        {
            var existingAccount = await this.provider.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Account", id)
                );
            }

            return existingAccount;
        }

        public async Task<ServiceResult<AccountModel>> UpdateAsync(Guid id, AccountModel account)
        {
            var existingAccountResult = await this.GetByIdAsync(id);
            if (existingAccountResult.HasError)
                return existingAccountResult.Error;

            var updatedAccount = await this.provider.UpdateAsync(id, account);
            if (updatedAccount == null)
            {
                return new InvalidDataError(
                    this.errorMessageProvider.UpdateFailedMessage("Account", id)
                );
            }

            return updatedAccount;
        }
    }
}
