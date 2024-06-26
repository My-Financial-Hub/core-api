﻿using FinancialHub.Core.Infra.Caching.Configurations;
using FinancialHub.Core.Infra.Caching.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Repositories
{
    internal class CategoriesCache : ICategoriesCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<CategoriesCache> logger;
        private const string PREFIX = "categories";

        private readonly CacheConfiguration config;

        public CategoriesCache(IDistributedCache cache, IOptions<CacheConfiguration> options, ILogger<CategoriesCache> logger)
        {
            this.cache = cache;
            this.config = options.Value;
            this.logger = logger;
        }

        public async Task AddAsync(CategoryModel category)
        {
            var id = category.Id;
            var key = $"{PREFIX}:{id}";
            this.logger.LogInformation("Adding category {id} to cache", id);
            this.logger.LogTrace("Adding key {key} to cache", key);

            await this.cache.SetAsync(
                key,
                category.ToByteArray(),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(config.ExpirationTime)
                }
            );

            this.logger.LogInformation("Category {id} added to cache", id);
        }

        public async Task<CategoryModel?> GetAsync(Guid id)
        {
            var key = $"{PREFIX}:{id}";
            this.logger.LogInformation("Getting category {id} from cache", id);
            this.logger.LogTrace("Getting key {key} from cache", key);

            var result = await this.cache.GetAsync(key);
            if (result == null || result.Length == 0)
            {
                this.logger.LogInformation("Category {id} not found in cache", id);
                return null;
            }

            this.logger.LogInformation("Category {id} found in cache", id);
            return result.FromByteArray<CategoryModel>();
        }

        public async Task RemoveAsync(Guid id)
        {
            this.logger.LogInformation("Removing category {id} from cache", id);
            await this.cache.RemoveAsync($"{PREFIX}:{id}");
            this.logger.LogInformation("Category {id} removed from cache", id);
        }
    }
}
