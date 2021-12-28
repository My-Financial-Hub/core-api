using FinancialHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class TransactionsRepositoryTests
    {
        protected override void AssertCreated(TransactionEntity createdItem)
        {
            base.AssertCreated(createdItem);

            Assert.IsNotNull(createdItem.Account);
            Assert.IsNotNull(createdItem.Category);
            Assert.AreEqual(createdItem.AccountId, createdItem.Account.Id);
            Assert.AreEqual(createdItem.CategoryId, createdItem.Category.Id);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with valid Category and Account", Category = "Create")]
        public override async Task CreateAsync_ValidItem_AddsOneRow(TransactionEntity item = null)
        {
            var entity = this.GenerateObject();

            entity.Category = await this.InsertData(entity.Category);
            entity.Account = await this.InsertData(entity.Account);

            await base.CreateAsync_ValidItem_AddsOneRow(entity);
        }

        [Test]
        [TestCase(TestName = "Create new Item with id", Category = "Create")]
        public async override Task CreateAsync_ValidItemWithId_AddsOneRowWithTheDifferentId(TransactionEntity item = null)
        {
            var entity = this.GenerateObject();

            var generatedAccount = this.GenerateAccount(entity.AccountId);
            var generatedCategory = this.GenerateCategory(entity.CategoryId);

            await this.InsertData(generatedAccount);
            await this.InsertData(generatedCategory);

            await base.CreateAsync_ValidItemWithId_AddsOneRowWithTheDifferentId(entity);
        }

        [Test]
        [TestCase(TestName = "Create item with existing id", Category = "Create")]
        public override async Task CreateAsync_ValidItemWithExistingId_AddsOneRowWithTheDifferentId(TransactionEntity item = null)
        {
            var id = Guid.NewGuid();
            item = this.GenerateObject(id);

            //INSERTS ACCOUNT AND CATEGORY
            var generatedAccount = this.GenerateAccount(item.AccountId);
            var generatedCategory = this.GenerateCategory(item.CategoryId);

            await this.InsertData(generatedAccount);
            await this.InsertData(generatedCategory);

            item.Category = null;
            item.Account = null;
            item = await this.InsertData(item);

            var newItem = this.GenerateObject(id);
            newItem.CategoryId  = generatedCategory.Id.GetValueOrDefault();
            newItem.AccountId   = generatedAccount.Id.GetValueOrDefault();

            var result = await this.repository.CreateAsync(newItem);

            Assert.AreNotEqual(item.Id,newItem.Id);
            this.AssertCreated(result);
            Assert.AreEqual(2, context.Set<TransactionEntity>().Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction without Updates/Creates Account or Category", Category = "Create")]
        public async Task CreateAsync_ValidItemWithNestChild_DoesNotUpdateNestChild()
        {
            #warning this test is too complex

            /***** ARRANGE *****/
            var entity = this.GenerateObject();

            //INSERTS ACCOUNT AND CATEGORY
            var oldAccount = this.GenerateAccount(entity.AccountId);
            var oldCategory = this.GenerateCategory(entity.CategoryId);

            await this.InsertData(oldAccount);
            await this.InsertData(oldCategory);

            //CHANGES TRANSACTION'S CATEGORY AND ACCOUNT
            entity.Account.Name = Guid.NewGuid().ToString();
            entity.Category.Name = Guid.NewGuid().ToString();

            /***** ACT *****/

            var result = await this.repository.CreateAsync(entity);

            /***** ASSERT *****/

            this.AssertCreated(result);

            //SHOULD NOT CREATE ACCOUNTS OR CATEGORIES
            Assert.AreEqual(1,this.context.Accounts.Count());
            Assert.AreEqual(1,this.context.Categories.Count());

            var account = this.context.Accounts.FirstOrDefault(x => x.Id == entity.AccountId);
            var category = this.context.Categories.FirstOrDefault(x => x.Id == entity.CategoryId);
            
            //SHOULD NOT UPDATE DATABASE
            Assert.AreEqual(oldAccount, account);
            Assert.AreEqual(oldCategory, category);

            //SHOULD NOT RETURN THE WRONG ITEM
            Assert.AreEqual(oldAccount, result.Account);
            Assert.AreEqual(oldCategory, result.Category);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with invalid Account", Category = "Create")]
        public async Task CreateAsync_InvalidAccountId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(this.GenerateAccount());

            entity.Category = await this.InsertData(entity.Category);
            entity.CategoryId = entity.Category.Id.GetValueOrDefault();

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions);
            Assert.AreEqual(1, this.context.Accounts.Count());
            Assert.AreEqual(1, this.context.Categories.Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with invalid Category", Category = "Create")]
        public async Task CreateAsync_InvalidCategoryId_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Account);
            await this.InsertData(this.GenerateCategory());

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions);
            Assert.AreEqual(1, this.context.Accounts.Count());
            Assert.AreEqual(1, this.context.Categories.Count());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Account", Category = "Create")]
        public async Task CreateAsync_NoAccount_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Category);

            entity.Account = null;

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions.ToList());
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Category", Category = "Create")]
        public async Task CreateAsync_NoCategory_ThrowsDbUpdateException()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Account);

            entity.Category = null;

            Assert.ThrowsAsync<DbUpdateException>(async () => await this.repository.CreateAsync(entity));

            Assert.IsEmpty(this.context.Transactions.ToList());
        }
    }
}
