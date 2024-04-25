using FinancialHub.Core.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

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

            this.cache = new CategoriesCache(
                distributedCache.Object, 
                logger.Object
            );
        }
    }
}
