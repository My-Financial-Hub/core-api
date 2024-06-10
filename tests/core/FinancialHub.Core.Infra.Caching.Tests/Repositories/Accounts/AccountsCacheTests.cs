using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Caching.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class AccountsCacheTests
    {
        private IAccountsCache cache;
        private Mock<IDistributedCache> distributedCache;
        private Mock<ILogger<AccountsCache>> logger;

        private AccountModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            this.builder            = new AccountModelBuilder();

            this.distributedCache   = new Mock<IDistributedCache>();
            this.logger             = new Mock<ILogger<AccountsCache>>();

            var config = new Mock<IOptions<CacheConfiguration>>();
            config.SetupGet(x => x.Value).Returns(new CacheConfiguration() { ExpirationTime = 600 });
            this.cache = new AccountsCache(
                distributedCache.Object,
                config.Object,
                logger.Object
            );
        }
    }
}
