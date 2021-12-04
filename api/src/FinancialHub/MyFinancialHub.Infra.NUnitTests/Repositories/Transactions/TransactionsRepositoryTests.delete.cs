using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Transactions
{
    public partial class TransactionsRepositoryTests
    {
        [Test]
        [TestCase(TestName = "Delete a Transaction without delete Category or Account", Category = "Delete")]
        public async Task DeleteAsync_ValidItemWithNestChild_DoesNotDeleteNestChild()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Account);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);

            var result = await this.repository.DeleteAsync(entity.Id.Value);

            Assert.AreEqual(1,result);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);
        }
    }
}
