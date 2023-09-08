namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class CategoriesProviderTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsCategoryList()
        {
            var categoriesEntity = categoryBuilder.Generate(random.Next(0, 10));
            var expectedCategories = mapper.Map<IEnumerable<CategoryModel>>(categoriesEntity);

            repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(categoriesEntity);

            var categories = await provider.GetAllAsync();

            CategoryModelAssert.Equal(expectedCategories.ToArray(), categories.ToArray());
        }

        [Test]
        public async Task GetByIdAsync_ExistingCategory_ReturnsCategory()
        {
            var id = Guid.NewGuid();
            var categoryEntity = categoryBuilder
                .WithId(id)
                .Generate();
            var expectedCategory = mapper.Map<CategoryModel>(categoryEntity);

            repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(categoryEntity);

            var result = await provider.GetByIdAsync(id);
        
            CategoryModelAssert.Equal(expectedCategory, result);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingCategory_ReturnsNull()
        {
            var id = Guid.NewGuid();
            var categoryEntity = categoryBuilder
                .WithId(id)
                .Generate();

            var result = await provider.GetByIdAsync(id);

            Assert.That(result, Is.Null);
        }
    }
}
