using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Providers
{
    internal class AccountsProvider : IAccountsProvider
    {
        private readonly IAccountsRepository repository;
        private readonly IAccountsCache cache;
        private readonly IBalancesProvider balanceProvider;
        private readonly IMapper mapper;
        private readonly ILogger<AccountsProvider> logger;

        public AccountsProvider(
            IAccountsRepository repository, IAccountsCache cache,
            IBalancesProvider balanceProvider,
            IMapper mapper,
            ILogger<AccountsProvider> logger
        )
        {
            this.repository = repository;
            this.cache = cache;
            this.balanceProvider = balanceProvider;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);

            this.logger.LogInformation("Creating account \"{Name}\"", account.Name);
            var createdAccount = await this.repository.CreateAsync(accountEntity);

            var balance = BalanceModel.CreateDefault(
                createdAccount.Id.GetValueOrDefault(), 
                createdAccount.Name, 
                createdAccount.IsActive
            );
            this.logger.LogInformation("Creating default balance \"{BalanceName}\" to account \"{AccountName}\"", balance.Name, account.Name);
            await this.balanceProvider.CreateAsync(balance);
            this.logger.LogInformation("Default Balance \"{BalanceName}\" created in account \"{AccountName}\"", account.Name, account.Name);

            var accountModel =  mapper.Map<AccountModel>(createdAccount);
            await this.cache.AddAsync(accountModel);

            await this.repository.CommitAsync();
            this.logger.LogInformation("Account \"{Name}\" created", account.Name);

            return accountModel;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing account {id}", id);
            await repository.DeleteAsync(id);

            var removedLines = await this.repository.CommitAsync();

            await cache.RemoveAsync(id);

            this.logger.LogInformation("Account {removed} removed", removedLines == 0? "not": id);
            return removedLines;
        }

        public async Task<ICollection<AccountModel>> GetAllAsync()
        {
            var accounts = await this.repository.GetAllAsync();

            return mapper.Map<ICollection<AccountModel>>(accounts);
        }

        public async Task<AccountModel?> GetByIdAsync(Guid id)
        {
            var cachedAccount = await this.cache.GetAsync(id);
            if (cachedAccount != null)
            { 
                return cachedAccount;
            }

            var accountEntity = await this.repository.GetByIdAsync(id);
            if (accountEntity == null)
            {
                return null;
            }

            var account = mapper.Map<AccountModel>(accountEntity);
            await this.cache.AddAsync(account);
            
            return account;
        }

        public async Task<AccountModel> UpdateAsync(Guid id, AccountModel account)
        {
            var accountEntity = mapper.Map<AccountEntity>(account);
            accountEntity.Id = id;

            var updatedAccount = await this.repository.UpdateAsync(accountEntity);
            await this.repository.CommitAsync();
            await cache.RemoveAsync(id);

            return mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
