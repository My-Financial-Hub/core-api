using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

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

            this.cache = new AccountsCache(
                distributedCache.Object, 
                logger.Object
            );
        }
    }
}
