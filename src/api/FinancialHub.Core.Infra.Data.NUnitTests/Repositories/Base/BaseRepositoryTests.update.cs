using FinancialHub.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialHub.Core.Infra.Data.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T>
        where T : BaseEntity
    {
        [Test]
        [TestCase(TestName = "Update existing Item", Category = "Update")]
        public virtual async Task UpdateAsync_ExistingItem_UpdatesItem()
        {
            var item = this.GenerateObject();
            await this.InsertData(new T[1] { item });

            var oldCreationTime = item.UpdateTime.GetValueOrDefault();

            var updatedItem = await this.repository.UpdateAsync(item);

            Assert.IsNotNull(updatedItem);
            Assert.IsNotNull(updatedItem.Id);
            Assert.IsNotNull(updatedItem.CreationTime);
            Assert.IsNotNull(updatedItem.UpdateTime);
            Assert.AreNotEqual(oldCreationTime,updatedItem.UpdateTime);
            Assert.IsInstanceOf<T>(updatedItem);
        }

        [Test]
        [TestCase(TestName = "Update non existing Item", Category = "Update")]
        public virtual void UpdateAsync_NonExistingItem_ThrowsDbUpdateConcurrencyException()
        {
            var id = Guid.NewGuid();
            var item = this.GenerateObject(id);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(item));
        }
    }
}
