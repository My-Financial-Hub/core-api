using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Domain.Tests.Builders.Models;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class CategoriesCacheTests
    {
        private ICategoriesCache cache;
        private IMock<IDistributedCache> distributedCache;
        private IMock<ILogger<CategoriesCache>> logger;

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
