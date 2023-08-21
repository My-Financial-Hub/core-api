namespace FinancialHub.Core.Infra.Data.Tests.Repositories
{
    public partial class TransactionsRepositoryTests
    {
        [Test]
        public async Task DeleteAsync_DoesNotDeleteBalance()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Balance);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);
            this.context.ChangeTracker.Clear();

            var result = await this.repository.DeleteAsync(entity.Id.Value);

            Assert.AreEqual(1,result);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Balances.Local.Count);
        }

        [Test]
        public async Task DeleteAsync_DoesNotDeleteCategory()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Balance);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);
            this.context.ChangeTracker.Clear();

            var result = await this.repository.DeleteAsync(entity.Id.Value);

            Assert.AreEqual(1, result);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Categories.Local.Count);
        }
    }
}
