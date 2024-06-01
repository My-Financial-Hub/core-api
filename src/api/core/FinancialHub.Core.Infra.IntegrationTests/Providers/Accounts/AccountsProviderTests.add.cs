namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task CreateAsync_AddsToDatabase()
        {
            var databaseAccountCount = database.Accounts.Count();
            var account = this.builder.Generate();

            await this.provider.CreateAsync(account);

            Assert.Multiple(() =>
            {
                Assert.That(database.Accounts.Count(), Is.EqualTo(databaseAccountCount + 1));
                Assert.That(
                    database.Accounts.FirstOrDefault(x => 
                        x.Name == account.Name && 
                        x.Description == account.Description && 
                        x.IsActive == account.IsActive
                    ),
                    Is.Not.Null
                );
            });
        }

        [Test]
        public async Task CreateAsync_AddsToCache()
        {
            var account = this.builder.Generate();

            await this.provider.CreateAsync(account);

            var data = database.Accounts
                .FirstOrDefault(x => 
                    x.Name == account.Name && 
                    x.Description == account.Description && 
                    x.IsActive == account.IsActive
                );
            var cacheData = await cache.GetAsync($"accounts:{data!.Id}");
            
            Assert.That(cacheData, Is.Not.Empty);
        }
    }
}
