using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    public class AccountCache : IAccountCache
    {
        private readonly IDistributedCache cache;

        public AccountCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task AddAsync(Guid id, AccountModel account)
        {
            await this.cache.SetAsync(
                id.ToString(), 
                account.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000 * 60)
                }
            );
        }

        public async Task<AccountModel?> GetAsync(Guid id)
        {
            var result = await this.cache.GetAsync(id.ToString());
            if(result == null)
            {
                return null;
            }

            return result.FromByteArray<AccountModel>();
        }

        public async Task RemoveAsync(Guid id)
        {
            await this.cache.RemoveAsync(id.ToString());
        }
    }
}
