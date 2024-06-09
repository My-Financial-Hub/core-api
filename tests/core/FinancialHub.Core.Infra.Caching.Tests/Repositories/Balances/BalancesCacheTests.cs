using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

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

            this.cache = new BalancesCache(
                distributedCache.Object, 
                logger.Object
            );
        }
    }
}
