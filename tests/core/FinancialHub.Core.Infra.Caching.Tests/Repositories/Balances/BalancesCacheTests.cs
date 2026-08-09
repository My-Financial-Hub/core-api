using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Caching.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class BalancesCacheTests
    {
        private IBalancesCache cache;
        private Mock<IDistributedCache> distributedCache;

        private BalanceModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            this.builder            = new BalanceModelBuilder();

            this.distributedCache   = new Mock<IDistributedCache>();

            this.cache = new BalancesCache(
                distributedCache.Object,
                Options.Create(new CacheConfiguration() { ExpirationTime = 600 }),
                new NullLoggerFactory().CreateLogger<BalancesCache>()
            );
        }
    }
}
