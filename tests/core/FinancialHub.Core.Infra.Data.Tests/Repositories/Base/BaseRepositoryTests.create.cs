using FinancialHub.Common.Entities;

namespace FinancialHub.Core.Infra.Data.Tests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> 
        where T : BaseEntity
    {
        protected virtual void AssertCreated(T createdItem)
        {
            Assert.IsNotNull(createdItem);
            Assert.IsNotNull(createdItem.Id);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);
            Assert.IsInstanceOf<T>(createdItem);

            Assert.IsNotEmpty(context.Set<T>().ToList());
        }

        [Test]
        [TestCase(TestName = "Create new Item", Category = "Create")]
        public virtual async Task CreateAsync_ValidItem_AddsOneRow(T item = null)
        {
            item ??= this.GenerateObject();

            var createdItem = await this.repository.CreateAsync(item);
            await repository.CommitAsync();

            this.AssertCreated(createdItem);
        }

        [Test]
        [TestCase(TestName = "Create new Item with id", Category = "Create")]
        public virtual async Task CreateAsync_ValidItemWithId_AddsOneRowWithTheDifferentId(T item = null)
        {
            var id = item == null ? Guid.NewGuid() : item.Id.GetValueOrDefault();
            item ??= this.GenerateObject(id);

            var createdItem = await this.repository.CreateAsync(item);
            await repository.CommitAsync();

            this.AssertCreated(createdItem);
            Assert.AreNotEqual(id,createdItem.Id);
        }

        [Test]
        [TestCase(TestName = "Create item with existing id", Category = "Create")]
        public virtual async Task CreateAsync_ValidItemWithExistingId_AddsOneRowWithTheDifferentId(T item = null)
        {
            var id = item == null ? Guid.NewGuid() : item.Id.GetValueOrDefault();

            item ??= this.GenerateObject(id);
            await this.InsertData(item);

            item ??= this.GenerateObject(id);

            var result = await this.repository.CreateAsync(item);
            await repository.CommitAsync();

            this.AssertCreated(result);
            Assert.AreEqual(2,context.Set<T>().Count());
        }
    }
}
