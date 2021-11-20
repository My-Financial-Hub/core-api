using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Transactions
{
    public partial class TransactionsRepositoryTests
    {
        protected async Task InsertTransaction(TransactionEntity entity)
        {
            await this.InsertData(entity.Account);
            await this.InsertData(entity.Category);
            await this.InsertData(entity);
        }

        [Test]
        [TestCase(TestName = "Update Transaction but no Accounts or Category", Category = "Update")]
        public async Task UpdateAsync_ValidItemWithNestChild_DoesNotUpdateNestChild()
        {
            var entity = this.GenerateObject();

            var oldEntityDescription = entity.Description;
            var oldAccountName = entity.Account.Name;
            var oldCategoryName = entity.Category.Name;

            entity.Description = Guid.NewGuid().ToString();
            entity.Account.Name = Guid.NewGuid().ToString();
            entity.Category.Name = Guid.NewGuid().ToString();

            await this.InsertTransaction(entity);

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);
            Assert.AreEqual(result.Description, entity.Description);

            //SHOULD NOT CREATE 
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);

            var account = this.context.Accounts.FirstOrDefault(x => x.Id == entity.AccountId);
            var category = this.context.Categories.FirstOrDefault(x => x.Id == entity.CategoryId);

            //SHOULD NOT UPDATE DATABASE
            Assert.AreEqual(oldAccountName, account.Name);
            Assert.AreEqual(oldCategoryName, category.Name);

            //SHOULD NOT RETURN THE WRONG NAME
            Assert.AreEqual(oldAccountName, result.Account.Name);
            Assert.AreEqual(oldCategoryName, result.Category.Name);
        }

        [Test]
        [TestCase(TestName = "Update Transaction changing Account", Category = "Update")]
        public async Task UpdateAsync_ChangingAccountId_ChangesAccount()
        {
            var entity = this.GenerateObject();
            var newAccount = this.GenerateAccount();

            await this.InsertTransaction(entity);
            await this.InsertData(newAccount);

            entity.AccountId = (Guid)newAccount.Id;

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);

            Assert.AreEqual(newAccount.Id, result.AccountId);
            Assert.AreEqual(newAccount, result.Account);
        }

        [Test]
        [TestCase(TestName = "Update Transaction changing Category", Category = "Update")]
        public async Task UpdateAsync_ChangingCategoryId_Throws()
        {
            var entity = this.GenerateObject();
            var newCategory = this.GenerateCategory();

            await this.InsertTransaction(entity);
            await this.InsertData(newCategory);

            entity.CategoryId = (Guid)newCategory.Id;

            var result = await this.repository.CreateAsync(entity);

            this.AssertCreated(result);

            Assert.AreEqual(newCategory.Id, result.CategoryId);
            Assert.AreEqual(newCategory, result.Category);
        }

        [Test]
        [TestCase(TestName = "Update Transaction with invalid Account", Category = "Update")]
        public async Task UpdateAsync_InvalidAccountId_Throws()
        {
            var entity = this.GenerateObject();
            var oldCategoryId = entity.Category.Id;
            var newCategory = this.GenerateCategory();

            await this.InsertTransaction(entity);

            entity.CategoryId = (Guid)newCategory.Id;

            Assert.ThrowsAsync<DbUpdateConcurrencyException>( async () =>await this.repository.UpdateAsync(entity));
        }

        [Test]
        [TestCase(TestName = "Update Transaction with invalid Category", Category = "Update")]
        public async Task UpdateAsync_InvalidCategoryId_Throws()
        {
            var entity = this.GenerateObject();
            var newCategory = this.GenerateCategory();

            await this.InsertTransaction(entity);

            entity.CategoryId = (Guid)newCategory.Id;
            entity.Category = newCategory;

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(entity));
        }

        [Test]
        [TestCase(TestName = "Update Transaction with no Account", Category = "Update")]
        public async Task UpdateAsync_NoAccountId_Throws()
        {
            throw new NotImplementedException();
        }

        [Test]
        [TestCase(TestName = "Update Transaction with no Category", Category = "Update")]
        public async Task UpdateAsync_NoCategoryId_Throws()
        {
            throw new NotImplementedException();
        }
    }
}
