using FinancialHub.Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        #region Delete
        [Test]
        [TestCase(TestName = "Delete Existing Item",Category = "Delete")]
        public async Task DeleteAsync_ExistingItem_AffectsOneRow()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var affectedRows = await this.repository.DeleteAsync(items.First().Id.ToString());
            Assert.AreEqual(1,affectedRows);
        }

        [Test]
        [TestCase(TestName = "Delete Non Existing Item",Category = "Delete")]
        public async Task DeleteAsync_NonExistingItem_AffectsNothing()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var affectedRows = await this.repository.DeleteAsync(new Guid().ToString());
            Assert.AreEqual(0, affectedRows);
        }

        [Test]
        [TestCase(TestName = "Delete Null id Item", Category = "Delete")]
        public async Task DeleteAsync_NullId_AffectsNothing()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var affectedRows = await this.repository.DeleteAsync(null);
            Assert.AreEqual(0, affectedRows);
        }
        #endregion
    }
}
