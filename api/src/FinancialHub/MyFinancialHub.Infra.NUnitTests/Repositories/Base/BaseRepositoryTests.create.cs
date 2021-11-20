using System;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.Domain.Entities;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        protected virtual void AssertCreated(T createdItem)
        {
            Assert.IsNotNull(createdItem);
            Assert.IsNotNull(createdItem.Id);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);
            Assert.IsInstanceOf<T>(createdItem);

            Assert.IsNotEmpty(context.Set<T>().Local);
        }

        #region Create
        [Test]
        [TestCase(TestName = "Create new Item", Category = "Create")]
        public virtual async Task CreateAsync_ValidItem_AddsOneRow(T item = null)
        {
            item ??= this.GenerateObject();

            var createdItem = await this.repository.CreateAsync(item);

            this.AssertCreated(createdItem);
        }

        [Test]
        [TestCase(TestName = "Create new Item with id", Category = "Create")]
        public virtual async Task CreateAsync_ValidItemWithId_AddsOneRow(T item = null)
        {
            var id = Guid.NewGuid();
            item ??= this.GenerateObject(id);

            var createdItem = await this.repository.CreateAsync(item);

            this.AssertCreated(createdItem);

            Assert.AreNotEqual(id,createdItem.Id);
        }

        [Test]
        [TestCase(TestName = "Create existing item", Category = "Create")]
        public virtual async Task CreateAsync_ExistingItemWith_AddsOneRow(T item = null)//TODO: verify result
        {
            var id = Guid.NewGuid();
            item ??= this.GenerateObject(id);

            await this.InsertData(item);

            var createdItem = await this.repository.CreateAsync(item);

            this.AssertCreated(createdItem);

            Assert.AreNotEqual(id, createdItem.Id);
        }

        [Test]
        [TestCase(TestName = "Create null Item", Category = "Create")]
        public virtual async Task CreateAsync_NullItem_ThrowsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await this.repository.CreateAsync(null));
            Assert.IsEmpty(context.Set<T>().Local);
        }
        #endregion
    }
}
