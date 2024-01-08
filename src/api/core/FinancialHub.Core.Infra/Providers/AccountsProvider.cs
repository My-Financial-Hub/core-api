namespace FinancialHub.Core.Infra.Providers
{
    public class AccountsProvider : IAccountsProvider
    {
        private readonly IMapper mapper;
        private readonly IAccountsRepository repository;
        private readonly IBalancesRepository balanceRepository;

        public AccountsProvider(IMapper mapper, IAccountsRepository repository, IBalancesRepository balanceRepository)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.balanceRepository = balanceRepository;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);
            var createdAccount = await this.repository.CreateAsync(accountEntity);

            var balance = new BalanceModel()
            {
                Name = $"{createdAccount!.Name} Default Balance",
                AccountId = createdAccount.Id.GetValueOrDefault(),
                IsActive = createdAccount.IsActive
            };
            var balanceEntity = mapper.Map<BalanceEntity>(balance);
            await this.balanceRepository.CreateAsync(balanceEntity);

            await this.repository.CommitAsync();

            return mapper.Map<AccountModel>(createdAccount);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
            return await this.repository.CommitAsync();
        }

        public async Task<ICollection<AccountModel>> GetAllAsync()
        {
            var accounts = await this.repository.GetAllAsync();

            return mapper.Map<ICollection<AccountModel>>(accounts);
        }

        public async Task<AccountModel?> GetByIdAsync(Guid id)
        {
            var account = await this.repository.GetByIdAsync(id);

            if (account == null)
                return null;

            return mapper.Map<AccountModel>(account);
        }

        public async Task<AccountModel> UpdateAsync(Guid id, AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);
            accountEntity.Id = id;

            var updatedAccount = await this.repository.UpdateAsync(accountEntity);
            await this.repository.CommitAsync();
            
            return mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
