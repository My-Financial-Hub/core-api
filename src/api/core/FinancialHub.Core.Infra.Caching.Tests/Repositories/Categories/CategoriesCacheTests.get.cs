using FinancialHub.Core.Domain.Tests.Assertions.Models;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class CategoriesCacheTests
    {
        [Test]
        public async Task GetAsync_ExistingCategory_ReturnsCategory()
        {
            var guid = Guid.NewGuid();
            var expectedResult = this.builder.Generate();

            var category = await this.cache.GetAsync(guid);

            CategoryModelAssert.Equal(expectedResult, category);
        }

        [Test]
        public async Task GetAsync_NotExistingCategory_ReturnsNull()
        {
            var guid = Guid.NewGuid();

            var category = await this.cache.GetAsync(guid);

            Assert.That(category, Is.Null);
        }
    }
}
