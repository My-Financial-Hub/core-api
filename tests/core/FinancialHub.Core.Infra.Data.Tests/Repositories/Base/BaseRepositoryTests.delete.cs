﻿using FinancialHub.Common.Entities;

namespace FinancialHub.Core.Infra.Data.Tests.Repositories.Base
{
    public abstract partial class BaseRepositoryTests<T>
        where T : BaseEntity
    {
        [Test]
        [TestCase(TestName = "Delete existing Item", Category = "Delete")]
        public virtual async Task DeleteAsync_ExistingItem_AffectsOneRow()
        {
            var items = this.GenerateData();
            await this.InsertData(items);
            this.context.ChangeTracker.Clear();

            var affectedRows = await this.repository.DeleteAsync(items.First().Id.Value);
            await repository.CommitAsync();

            Assert.AreEqual(1, affectedRows);
            Assert.AreEqual(items.Count - 1, context.Set<T>().ToList().Count);
        }

        [Test]
        [TestCase(TestName = "Delete non existing Item", Category = "Delete")]
        public virtual async Task DeleteAsync_NonExistingItem_AffectsNothing()
        {
            var items = this.GenerateData();
            await this.InsertData(items);
            this.context.ChangeTracker.Clear();

            var affectedRows = await this.repository.DeleteAsync(Guid.Empty);
            Assert.AreEqual(0, affectedRows);
            Assert.IsNotEmpty(context.Set<T>().ToList());
        }
    }
}
