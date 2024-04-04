using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    public class BalancesCache : IBalancesCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<BalancesCache> logger;
        private const string PREFIX = "accounts:{0}:balances";

        public BalancesCache(IDistributedCache cache, ILogger<BalancesCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public async Task AddAsync(BalanceModel balance)
        {
            var id = balance.Id;
            var prefix = string.Format(PREFIX, balance.AccountId);
            var key = $"{prefix}:{id}";

            this.logger.LogInformation("Adding balance {id} to cache", id);
            this.logger.LogTrace("Adding key {key} to cache", key);

            await this.cache.SetAsync(
                key,
                balance.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                }
            );

            this.logger.LogInformation("Balance {id} to cache", id);
        }

        public async Task<BalanceModel?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BalanceModel>> GetByAccountAsync(Guid accountId)
        {
            var prefix = string.Format(PREFIX, accountId);

            var result = await this.cache.GetAsync(prefix);
            if (result == null)
            {
                this.logger.LogInformation("Balances in account {id} not found in cache", accountId);
                return Array.Empty<BalanceModel>();
            }

            this.logger.LogInformation("Balances from account {id} found in cache", accountId);
            return result.FromByteArray<BalanceModel[]>() ?? Array.Empty<BalanceModel>();
        }

        public async Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
