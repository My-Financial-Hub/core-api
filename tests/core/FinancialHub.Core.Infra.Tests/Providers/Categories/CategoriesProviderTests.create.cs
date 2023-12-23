namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class CategoriesProviderTests
    {
        [Test]
        public async Task CreateAsync_ReturnsCreatedCategory()
        {
            var category = this.categoryModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<CategoryEntity>()))
                .Returns<CategoryEntity>(async x => await Task.FromResult(x));

            var result = await this.provider.CreateAsync(category);

            Assert.That(result, Is.Not.Null);
            CategoryModelAssert.Equal(category, result);
        }

        [Test]
        public async Task CreateAsync_CallsCategoryRepository()
        {
            var category = this.categoryModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<CategoryEntity>()))
                .Returns<CategoryEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(category);

            repository.Verify(x => x.CreateAsync(It.IsAny<CategoryEntity>()), Times.Once);
        }
    }
}
