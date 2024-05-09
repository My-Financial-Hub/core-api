namespace FinancialHub.Core.Infra.IntegrationTests.Providers.Accounts
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsFromDatabase()
        {
            var entity = this.entitybuilder.Generate();
            await this.database.AddAsync(entity);
            await this.database.SaveChangesAsync();

            var result = await this.provider.GetAllAsync();

            Assert.That(result,Is.Not.Empty);
        }
    }
}
