using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    public class AccountsCache : IAccountsCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<AccountsCache> logger;
        private const string PREFIX = "accounts";

        public AccountsCache(IDistributedCache cache, ILogger<AccountsCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public async Task AddAsync(AccountModel account)
        {
            var id = account.Id;
            var key = $"{PREFIX}:{id}";
            this.logger.LogInformation("Adding account {id} to cache", id);
            this.logger.LogTrace("Adding key {key} to cache", key);
            
            await this.cache.SetAsync(
                key, 
                account.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                }
            );
           
            this.logger.LogInformation("Account {id} to cache", id);
        }

        public async Task<AccountModel?> GetAsync(Guid id)
        {
            var key = $"{PREFIX}:{id}";
            this.logger.LogInformation("Getting account {id} from cache", id);
            this.logger.LogTrace("Getting key {key} from cache", key);
           
            var result = await this.cache.GetAsync(key);
            if(result == null || result.Length == 0)
            {
                this.logger.LogInformation("Account {id} not found in cache", id);
                return null;
            }

            this.logger.LogInformation("Account {id} found in cache", id);
            return result.FromByteArray<AccountModel>();
        }

        public async Task RemoveAsync(Guid id)
        {
            this.logger.LogInformation("Removing account {id} from cache", id);
            await this.cache.RemoveAsync($"{PREFIX}:{id}");
            this.logger.LogInformation("Account {id} removed from cache", id);
        }
    }
}
