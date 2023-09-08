namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class CategoriesProviderTests
    {

        [Test]
        public async Task UpdateAsync_ReturnsUpdatedCategory()
        {
            var id = Guid.NewGuid();
            var category = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            repository
                .Setup(x => x.UpdateAsync(It.Is<CategoryEntity>(x => x.Id == id)))
                .Returns<CategoryEntity>(async x => await Task.FromResult(x));

            var result = await provider.UpdateAsync(id, category);

            CategoryModelAssert.Equal(category, result);
        }

        [Test]
        public async Task UpdateAsync_CallsCategoryRepository()
        {
            var id = Guid.NewGuid();
            var category = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, category);

            repository.Verify(x => x.UpdateAsync(It.IsAny<CategoryEntity>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsCategoryRepositoryWithIDFromParam()
        {
            var id = Guid.NewGuid();
            var category = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, category);
            repository.Verify(x => x.UpdateAsync(It.Is<CategoryEntity>(x => x.Id == id)), Times.Once);
        }
    }
}
