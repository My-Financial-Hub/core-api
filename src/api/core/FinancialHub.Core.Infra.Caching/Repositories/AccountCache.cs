using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    public class AccountCache : IAccountCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<AccountCache> logger;
        private const string PREFIX = "accounts";

        public AccountCache(IDistributedCache cache, ILogger<AccountCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public async Task AddAsync(Guid id, AccountModel account)
        {
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
            if(result == null)
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
