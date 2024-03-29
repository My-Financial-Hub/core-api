﻿using Microsoft.EntityFrameworkCore;

namespace FinancialHub.Core.Infra.Data.Tests.Repositories
{
    public partial class TransactionsRepositoryTests
    {
        protected async Task InsertTransaction(TransactionEntity entity)
        {
            await this.InsertData(entity.Balance);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);

            context.Entry(entity).State = EntityState.Detached;
        }

        [Test]
        [TestCase(TestName = "Update Transaction but not Balance", Category = "Update")]
        public async Task UpdateAsync_ValidItemWithBalance_DoesNotUpdateBalance()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var changedEntity = this.GenerateTransaction(entity.Id, entity.BalanceId, entity.CategoryId);

            var result = await this.repository.UpdateAsync(changedEntity);
 
            this.AssertCreated(result);
            Assert.AreEqual(1, this.context.Balances.Local.Count);

            var balance = this.context.Balances.FirstOrDefault(x => x.Id == changedEntity.BalanceId);
            Assert.AreEqual(balance, result.Balance);
        }

        [Test]
        [TestCase(TestName = "Update Transaction but not Category", Category = "Update")]
        public async Task UpdateAsync_ValidItemWithCategory_DoesNotUpdateCategory()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var changedEntity = this.GenerateTransaction(entity.Id, entity.BalanceId, entity.CategoryId);

            var result = await this.repository.UpdateAsync(changedEntity);

            this.AssertCreated(result);
            Assert.AreEqual(1, this.context.Categories.Local.Count);

            var category = this.context.Categories.FirstOrDefault(x => x.Id == changedEntity.CategoryId);
            Assert.AreEqual(category, result.Category);
        }

        [Test]
        [TestCase(TestName = "Update Transaction changing Balance", Category = "Update")]
        public async Task UpdateAsync_ChangeBalanceId_ChangesBalance()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var newBalance = this.GenerateBalance();
            await this.InsertData(newBalance);

            entity.BalanceId = (Guid)newBalance.Id;

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);

            Assert.AreEqual(newBalance.Id, result.BalanceId);
            Assert.AreEqual(newBalance, result.Balance);
        }

        [Test]
        [TestCase(TestName = "Update Transaction changing Category", Category = "Update")]
        public async Task UpdateAsync_ChangeCategoryId_ChangesCategory()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var newCategory = this.GenerateCategory();
            await this.InsertData(newCategory);

            entity.CategoryId = (Guid)newCategory.Id;

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);

            Assert.AreEqual(newCategory.Id, result.CategoryId);
            Assert.AreEqual(newCategory, result.Category);
        }

        [Test]
        [TestCase(TestName = "Update Transaction with invalid Balance", Category = "Update")]
        public async Task UpdateAsync_InvalidBalanceId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertTransaction(entity);

            var newCategory = this.GenerateCategory();
            entity.CategoryId = (Guid)newCategory.Id;

            Assert.ThrowsAsync<DbUpdateException>( async () =>await this.repository.UpdateAsync(entity));
        }

        [Test]
        [TestCase(TestName = "Update Transaction with invalid Category", Category = "Update")]
        public async Task UpdateAsync_InvalidCategoryId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();
            var newCategory = this.GenerateCategory();

            await this.InsertTransaction(entity);

            entity.CategoryId = (Guid)newCategory.Id;
            entity.Category = newCategory;

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.UpdateAsync(entity));
        }
    }
}
