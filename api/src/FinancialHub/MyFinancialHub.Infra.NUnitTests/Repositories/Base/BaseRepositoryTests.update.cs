using FinancialHub.Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        #region Update
        [Test]
        [TestCase(TestName = "Update Existing Item", Category = "Update")]
        public async Task UpdateAsync_ExistingItem_UpdatesItem()
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
        public async Task UpdateAsync_NonExistingItem_DoesNotUpdate()
        {
            var id = Guid.NewGuid();
            var item = this.GenerateObject(id);

            var createdItem = await this.repository.CreateAsync(item);

            Assert.IsNotNull(createdItem);
            Assert.AreNotEqual(id, createdItem.Id);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);
            Assert.IsInstanceOf<T>(createdItem);
        }

        [Test]
        [TestCase(TestName = "Update Null Item", Category = "Update")]
        public async Task UpdateAsync_NullItem_ThrowsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await this.repository.UpdateAsync(null));
        }
        #endregion
    }
}
