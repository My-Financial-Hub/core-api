namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    public partial class CategoriesProviderTests
    {
        [Test]
        public async Task CreateAsync_AddsToDatabase()
        {
            var databaseCategoriesCount = database.Categories.Count();
            var category = this.builder.Generate();

            await this.provider.CreateAsync(category);

            Assert.Multiple(() =>
            {
                Assert.That(database.Categories.Count(), Is.EqualTo(databaseCategoriesCount + 1));
                Assert.That(
                    database.Categories.FirstOrDefault(x =>
                        x.Name == category.Name &&
                        x.Description == category.Description &&
                        x.IsActive == category.IsActive
                    ),
                    Is.Not.Null
                );
            });
        }

        [Test]
        public async Task CreateAsync_AddsToCache()
        {
            var category = this.builder.Generate();

            await this.provider.CreateAsync(category);

            var data = database.Categories
                .FirstOrDefault(x =>
                    x.Name == category.Name &&
                    x.Description == category.Description &&
                    x.IsActive == category.IsActive
                );
            var cacheData = await cache.GetAsync($"categories:{data!.Id}");

            Assert.That(cacheData, Is.Not.Empty);
        }
    }
}
