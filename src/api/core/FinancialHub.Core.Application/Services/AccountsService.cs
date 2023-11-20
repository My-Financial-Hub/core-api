namespace FinancialHub.Core.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsProvider provider;

        public AccountsService(IAccountsProvider provider)
        {
            this.provider = provider;
        }

        public async Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account)
        {
            return await this.provider.CreateAsync(account);
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
                return new NotFoundError($"Not found account with id {id}");

            return existingAccount;
        }

        public async Task<ServiceResult<AccountModel>> UpdateAsync(Guid id, AccountModel account)
        {
            var existingAccountResult = await this.GetByIdAsync(id);
            if (existingAccountResult.HasError)
                return existingAccountResult.Error;

            var updatedAccount = await this.provider.UpdateAsync(id, account);
            if (updatedAccount == null)
                return new NotFoundError($"Failed to update account {id}");

            return updatedAccount;
        }
    }
}
