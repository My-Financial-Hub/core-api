using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Caching.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class BalancesCacheTests
    {
        private IBalancesCache cache;
        private Mock<IDistributedCache> distributedCache;
        private Mock<ILogger<BalancesCache>> logger;

        private BalanceModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            this.builder            = new BalanceModelBuilder();

            this.distributedCache   = new Mock<IDistributedCache>();
            this.logger             = new Mock<ILogger<BalancesCache>>();

            var config              = new Mock<IOptions<CacheConfiguration>>();
            config.SetupGet(x => x.Value).Returns(new CacheConfiguration() { ExpirationTime = 600 });
            this.cache = new BalancesCache(
                distributedCache.Object,
                config.Object,
                logger.Object
            );
        }
    }
}
