using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Providers
{
    internal class AccountsProvider : IAccountsProvider
    {
        private readonly IAccountsRepository accountsRepository;
        private readonly IBalancesRepository balancesRepository;
        private readonly IAccountsCache cache;
        private readonly IMapper mapper;
        private readonly ILogger<AccountsProvider> logger;

        public AccountsProvider(
            IAccountsRepository accountsRepository, IBalancesRepository balancesRepository,
            IAccountsCache cache,
            IMapper mapper,
            ILogger<AccountsProvider> logger
        )
        {
            this.accountsRepository = accountsRepository;
            this.balancesRepository = balancesRepository;
            this.cache = cache;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var balance = BalanceModel.CreateDefault(account);

            this.logger.LogInformation("Creating account \"{Name}\"", account.Name);
            var createdAccount = await this.accountsRepository.CreateAsync(this.mapper.Map<AccountEntity>(account));

            this.logger.LogInformation("Creating default balance \"{BalanceName}\" to account \"{AccountName}\"", balance.Name, account.Name);
            await this.balancesRepository.CreateAsync(this.mapper.Map<BalanceEntity>(balance));

            await this.accountsRepository.CommitAsync();
            this.logger.LogInformation("Account \"{Name}\" created", account.Name);

            return this.mapper.Map<AccountModel>(createdAccount);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing account {id}", id);
            await accountsRepository.DeleteAsync(id);

            var removedLines = await this.accountsRepository.CommitAsync();

            await cache.RemoveAsync(id);

            this.logger.LogInformation("Account {removed} removed", removedLines == 0? "not": id);
            return removedLines;
        }

        public async Task<ICollection<AccountModel>> GetAllAsync()
        {
            var accounts = await this.accountsRepository.GetAllAsync();

            return mapper.Map<ICollection<AccountModel>>(accounts);
        }

        public async Task<AccountModel?> GetByIdAsync(Guid id)
        {
            var cachedAccount = await this.cache.GetAsync(id);
            if (cachedAccount != null)
            { 
                return cachedAccount;
            }

            var accountEntity = await this.accountsRepository.GetByIdAsync(id);
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

            var updatedAccount = await this.accountsRepository.UpdateAsync(accountEntity);
            await this.accountsRepository.CommitAsync();
            await cache.RemoveAsync(id);

            return mapper.Map<AccountModel>(updatedAccount);
        }
    }
}
