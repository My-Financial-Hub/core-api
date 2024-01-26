namespace FinancialHub.Core.Infra.Data.Tests.Repositories
{
    public partial class BalancesRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ShouldCreatesBalanceWithZeroAmount()
        {
            var item = this.GenerateObject();
            
            var createdItem = await this.repository.CreateAsync(item);
            await this.repository.CommitAsync();

            this.AssertCreated(createdItem);
            Assert.Zero(createdItem.Amount);

            var databaseItem = context.Set<BalanceEntity>().FirstOrDefault(x => x.Id == createdItem.Id);
            Assert.Zero(databaseItem.Amount);
        }
    }
}
