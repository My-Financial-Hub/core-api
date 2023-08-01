namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class BalancesRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ShouldCreatesBalanceWithZeroAmount()
        {
            var item = this.GenerateObject();
            
            var createdItem = await this.repository.CreateAsync(item);

            this.AssertCreated(createdItem);
            Assert.Zero(createdItem.Amount);

            var databaseItem = context.Set<BalanceEntity>().FirstOrDefault(x => x.Id == createdItem.Id);
            Assert.Zero(databaseItem.Amount);
        }
    }
}
