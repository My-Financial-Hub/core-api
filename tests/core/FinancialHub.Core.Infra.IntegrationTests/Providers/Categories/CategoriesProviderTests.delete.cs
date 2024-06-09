namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    public partial class CategoriesProviderTests
    {
        [Test]
        public async Task DeleteAsync_RemovesFromDatabase()
        {
            var entity = this.entitybuilder.Generate();
            await this.database.AddAsync(entity);
            await this.database.SaveChangesAsync();
            this.database.ChangeTracker.Clear();

            await this.provider.DeleteAsync(entity.Id.GetValueOrDefault());

            Assert.That(database.Categories.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_RemovesFromCache()
        {
            var entity = this.entitybuilder.Generate();
            await this.database.AddAsync(entity);
            await this.database.SaveChangesAsync();
            this.database.ChangeTracker.Clear();

            var id = entity.Id.GetValueOrDefault();
            await this.provider.DeleteAsync(id);

            var cacheData = await cache.GetAsync($"categories:{id}");

            Assert.That(cacheData, Is.Null);
        }
    }
}
