namespace FinancialHub.Core.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapperWrapper mapper;
        private readonly IAccountsRepository repository;
        private readonly IAccountsProvider provider;

        public AccountsService(IAccountsProvider provider, IMapperWrapper mapper,IAccountsRepository repository)
        {
            this.provider = provider;
            this.mapper = mapper;
            this.repository = repository;
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
            var entity = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<AccountModel>(entity);
        }

        public async Task<ServiceResult<AccountModel>> UpdateAsync(Guid id, AccountModel account)
        {
            var existingAccount = await this.provider.GetByIdAsync(id);
            if (existingAccount == null)
                return new NotFoundError($"Not found account with id {id}");

            var updatedAccount = await this.provider.UpdateAsync(id, account);
            if (updatedAccount == null)
                return new NotFoundError($"Failed to update account {id}");

            return mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
