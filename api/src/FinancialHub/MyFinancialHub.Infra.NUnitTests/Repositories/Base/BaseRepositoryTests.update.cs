using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        #region Update
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
        public virtual async Task UpdateAsync_NonExistingItem_ThrowsDbUpdateConcurrencyException()
        {
            var id = Guid.NewGuid();
            var item = this.GenerateObject(id);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(item));
        }
        #endregion
    }
}
