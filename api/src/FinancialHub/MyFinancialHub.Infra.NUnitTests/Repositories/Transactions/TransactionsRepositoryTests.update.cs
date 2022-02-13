using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories.Transactions
{
    public partial class TransactionsRepositoryTests
    {
        protected async Task InsertTransaction(TransactionEntity entity)
        {
            await this.InsertData(entity.Account);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);

            context.Entry(entity).State = EntityState.Detached;
        }

        [Test]
        [TestCase(TestName = "Update Transaction but no Accounts or Category", Category = "Update")]
        public async Task UpdateAsync_ValidItemWithNestChild_DoesNotUpdateNestChild()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var changedEntity = this.GenerateTransaction(entity.Id, entity.AccountId, entity.CategoryId);

            var result = await this.repository.UpdateAsync(changedEntity);

            this.AssertCreated(result);

            //SHOULD NOT CREATE 
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);

            var account = this.context.Accounts.FirstOrDefault(x => x.Id == changedEntity.AccountId);
            var category = this.context.Categories.FirstOrDefault(x => x.Id == changedEntity.CategoryId);

            //SHOULD NOT UPDATE DATABASE
            Assert.AreEqual(account, result.Account);
            Assert.AreEqual(category, result.Category);
        }

        [Test]
        [TestCase(TestName = "Update Transaction changing Account", Category = "Update")]
        public async Task UpdateAsync_ChangeAccountId_ChangesAccount()
        {
            var entity = this.GenerateObject();
            await this.InsertTransaction(entity);

            var newAccount = this.GenerateAccount();
            await this.InsertData(newAccount);

            entity.AccountId = (Guid)newAccount.Id;

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);

            Assert.AreEqual(newAccount.Id, result.AccountId);
            Assert.AreEqual(newAccount, result.Account);
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
        [TestCase(TestName = "Update Transaction with invalid Account", Category = "Update")]
        public async Task UpdateAsync_InvalidAccountId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();
            var oldCategoryId = entity.Category.Id;

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
