using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Caching.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class CategoriesCacheTests
    {
        private ICategoriesCache cache;
        private Mock<IDistributedCache> distributedCache;
        private Mock<ILogger<CategoriesCache>> logger;

        private CategoryModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            this.builder            = new CategoryModelBuilder();

            this.distributedCache   = new Mock<IDistributedCache>();
            this.logger             = new Mock<ILogger<CategoriesCache>>();

            var config = new Mock<IOptions<CacheConfiguration>>();
            config.SetupGet(x => x.Value).Returns(new CacheConfiguration() { ExpirationTime = 600 });
            this.cache = new CategoriesCache(
                distributedCache.Object,
                config.Object,
                logger.Object
            );
        }
    }
}
