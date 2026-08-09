using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Infra.Caching.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class AccountsCacheTests
    {
        private IAccountsCache cache;
        private Mock<IDistributedCache> distributedCache;

        private AccountModelBuilder builder;

        [SetUp]
        public void Setup()
        {
            this.builder            = new AccountModelBuilder();

            this.distributedCache   = new Mock<IDistributedCache>();

            this.cache = new AccountsCache(
                distributedCache.Object,
                Options.Create(new CacheConfiguration() { ExpirationTime = 600 }),
                new NullLoggerFactory().CreateLogger<AccountsCache>()
            );
        }
    }
}
