using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;

namespace FinancialHub.Infra.NUnitTests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T> where T : BaseEntity
    {
        #region Get All
        [Test]
        [TestCase(TestName = "Get All Items without data",Category = "Get")]
        public virtual async Task GetAllAsync_NoData_ReturnsEmpty()
        {
            var list = await this.repository.GetAllAsync();

            Assert.IsEmpty(list);
            Assert.IsInstanceOf<ICollection<T>>(list);
        }

        [Test]
        [TestCase(TestName = "Get All Items with data", Category = "Get")]
        public virtual async Task GetAllAsync_Data_ReturnsItems()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var list = await this.repository.GetAllAsync();

            Assert.IsNotEmpty(list);
            Assert.AreEqual(items,list);
            Assert.IsInstanceOf<ICollection<T>>(list);
        }
        #endregion

        #region Get
        [Test]
        [TestCase(TestName = "Get Items with no filter", Category = "Get")]
        public virtual async Task GetAsync_NoFilter_ReturnsAllItems()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var list = await this.repository.GetAsync((x) => true);

            Assert.IsNotEmpty(list);
            Assert.AreEqual(items, list);
            Assert.IsInstanceOf<ICollection<T>>(list);
        }

        [Test]
        [TestCase(TestName = "Get Items not setting filter", Category = "Get")]
        public virtual async Task GetAsync_NullFilter_ThrowsArgumentNullException()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            Assert.ThrowsAsync(typeof(ArgumentNullException), async () => await this.repository.GetAsync(null));
        }

        [Test]
        [TestCase(TestName = "Get Items filtering", Category = "Get")]
        public virtual async Task GetAsync_Filter_ReturnsFilteredItems()
        {
            var items = this.GenerateData(10,100);
            await this.InsertData(items);

            var id = items.First().Id;
            Func<T,bool> filter = (x) => x.Id == id;
            var expectedResult = items.Where(filter);

            var list = await this.repository.GetAsync(filter);

            Assert.IsNotEmpty(list);
            Assert.AreEqual(expectedResult, list);
            Assert.IsInstanceOf<ICollection<T>>(list);
        }

        [Test]
        [TestCase(TestName = "Get Items with wrong filter", Category = "Get")]
        public virtual async Task GetAsync_WrongFilter_ReturnsEmpty()
        {
            var items = this.GenerateData(10, 100);
            await this.InsertData(items);

            Func<T, bool> filter = (x) => x.Id == new Guid();

            var list = await this.repository.GetAsync(filter);

            Assert.IsEmpty(list);
            Assert.IsInstanceOf<ICollection<T>>(list);
        }
        #endregion

        #region Get By Id
        [Test]
        [TestCase(TestName = "Get By Id with empty id", Category = "Get")]
        public virtual async Task GetByIdAsync_EmptyId_ReturnsNull()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var item = await this.repository.GetByIdAsync("");

            Assert.IsNull(item);
        }

        [Test]
        [TestCase(TestName = "Get By Id with existing id", Category = "Get")]
        public virtual async Task GetByIdAsync_ExistingId_ReturnsItem()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var id = items.First().Id;

            var item = await this.repository.GetByIdAsync(id.ToString());

            Assert.IsNotNull(item);
            Assert.AreEqual(id,item.Id);
            Assert.IsInstanceOf<T>(item);
        }

        [Test]
        [TestCase(TestName = "Get By Id with non-existing id", Category = "Get")]
        public virtual async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var items = this.GenerateData();
            await this.InsertData(items);

            var item = await this.repository.GetByIdAsync(new Guid().ToString());

            Assert.IsNull(item);
        }
        #endregion
    }
}
