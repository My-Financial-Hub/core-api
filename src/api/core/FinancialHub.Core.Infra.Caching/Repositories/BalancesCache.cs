using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    public class BalancesCache : IBalancesCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<BalancesCache> logger;
        private const string ACCOUNT_PREFIX = "accounts";
        private const string BALANCE_PREFIX = "balances";

        public BalancesCache(IDistributedCache cache, ILogger<BalancesCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        private async Task AddToBalanceAsync(BalanceModel balance)
        {
            var id = balance.Id;

            this.logger.LogInformation("Adding balance {id} to cache", id);

            var key = $"{BALANCE_PREFIX}:{id}";
            this.logger.LogTrace("Adding key {key} to cache", key);
            await this.cache.SetAsync(
                key,
                balance.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                }
            );
            this.logger.LogTrace("Key {key} added to cache", key);

            this.logger.LogInformation("Balance {id} added to cache", id);
        }

        private async Task AddToAccountAsync(BalanceModel balance)
        {
            var id = balance.Id.GetValueOrDefault();
            var accountId = balance.AccountId;

            var balanceArray = await this.GetByAccountAsync(accountId);
            if(balanceArray == null)
                return;

            var balanceList = balanceArray.Where(x => x.Id != id).ToList();
            balanceList.Add(balance);

            this.logger.LogInformation("Adding balance {id} to account {id} cache", id, accountId);

            var key = $"{ACCOUNT_PREFIX}:{accountId}:balances";
            this.logger.LogTrace("Adding key {key} to cache", key);
            await this.cache.SetAsync(
                key,
                balance.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                }
            );
            this.logger.LogTrace("Key {key} added to cache", key);

            this.logger.LogInformation("Balance {id} added to account {id} cache", id, accountId);
        }

        public async Task AddAsync(BalanceModel balance)
        {
            await this.AddToBalanceAsync(balance);
            await this.AddToAccountAsync(balance);
        }

        public async Task AddAsync(IEnumerable<BalanceModel> balances)
        {
            var accountGroup = balances.GroupBy(x => x.AccountId);
            foreach (var item in accountGroup)
            {
                foreach (var balance in item)
                {
                    await this.AddToBalanceAsync(balance);
                }

                var key = $"{ACCOUNT_PREFIX}:{item.Key}:balances";
                await this.cache.SetAsync(
                    key,
                    balances.ToByteArray(),
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                    }
                );
            }
        }

        public async Task<ICollection<BalanceModel>?> GetByAccountAsync(Guid accountId)
        {
            var prefix = $"{ACCOUNT_PREFIX}:{accountId}:balances";
            var result = await this.cache.GetAsync(prefix);
            if (result == null)
            {
                this.logger.LogInformation("Balances in account {id} not found in cache", accountId);
                return null;
            }

            this.logger.LogInformation("Balances from account {id} found in cache", accountId);
            return result.FromByteArray<BalanceModel[]>() ?? Array.Empty<BalanceModel>();
        }

        public async Task RemoveAsync(Guid id)
        {
            var balance = await this.GetAsync(id);
            if(balance == null)
                return;

            var accountKey = $"{ACCOUNT_PREFIX}:{balance.AccountId}:balances";
            await this.cache.RemoveAsync(accountKey);

            var balanceKey = $"{BALANCE_PREFIX}:{id}";
            await this.cache.RemoveAsync(balanceKey);
        }

        public async Task<BalanceModel?> GetAsync(Guid id)
        {
            this.logger.LogInformation("Getting balance {id} from cache", id);

            var key = $"{BALANCE_PREFIX}:{id}";
            this.logger.LogTrace("Getting key {key} from cache", key);
            var result = await this.cache.GetAsync(key);
            if (result == null || result.Length == 0)
            {
                this.logger.LogInformation("Balance {id} not found in cache", id);
                return null;
            }
            this.logger.LogInformation("Balance {id} found in cache", id);

            return result.FromByteArray<BalanceModel>();
        }
    }
}
