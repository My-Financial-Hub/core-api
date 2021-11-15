using FinancialHub.Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        #region Create
        [Test]
        [TestCase(TestName = "Create new Item", Category = "Create")]
        public async Task CreateAsync_ValidItem_AddsOneRow()
        {
            var item = this.GenerateObject();

            var createdItem = await this.repository.CreateAsync(item);

            Assert.IsNotNull(createdItem);
            Assert.IsNotNull(createdItem.Id);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);
            Assert.IsInstanceOf<T>(createdItem);

            Assert.IsNotEmpty(context.Set<T>().Local);
        }

        [Test]
        [TestCase(TestName = "Create new Item with id", Category = "Create")]
        public async Task CreateAsync_ValidItemWithId_AddsOneRow()
        {
            var id = Guid.NewGuid();
            var item = this.GenerateObject(id);

            var createdItem = await this.repository.CreateAsync(item);

            Assert.IsNotNull(createdItem);
            Assert.AreNotEqual(id,createdItem.Id);
            Assert.IsNotNull(createdItem.CreationTime);
            Assert.IsNotNull(createdItem.UpdateTime);
            Assert.IsInstanceOf<T>(createdItem);

            Assert.IsNotEmpty(context.Set<T>().Local);
        }

        [Test]
        [TestCase(TestName = "Create null Item", Category = "Create")]
        public async Task CreateAsync_NullItem_ThrowsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await this.repository.CreateAsync(null));
            Assert.IsEmpty(context.Set<T>().Local);
        }
        #endregion
    }
}
