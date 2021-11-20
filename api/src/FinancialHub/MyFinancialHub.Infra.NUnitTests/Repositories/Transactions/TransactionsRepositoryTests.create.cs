using FinancialHub.Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialHub.Infra.NUnitTests.Repositories.Transactions
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

            await this.InsertData(entity.Category);
            await this.InsertData(entity.Account);

            await base.CreateAsync_ValidItem_AddsOneRow(entity);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction without Updates/Creates Account or Category", Category = "Create")]
        public async Task CreateAsync_ValidItemWithNestChild_DoesNotUpdateNestChild()
        {
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
            Assert.AreEqual(1,this.context.Accounts.Local.Count);
            Assert.AreEqual(1,this.context.Categories.Local.Count);

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
        public async Task CreateAsync_InvalidAccountId_Throws()
        {
            var entity = this.GenerateObject();

            await this.InsertData(this.GenerateAccount());
            await this.InsertData(entity.Category);

            var result = await this.repository.CreateAsync(entity);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with invalid Category", Category = "Create")]
        public async Task CreateAsync_InvalidCategoryId_Throws()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Account);
            await this.InsertData(this.GenerateCategory());

            var result = await this.repository.CreateAsync(entity);

            Assert.IsEmpty(this.context.Transactions.Local);
            Assert.AreEqual(1, this.context.Accounts.Local.Count);
            Assert.AreEqual(1, this.context.Categories.Local.Count);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Account", Category = "Create")]
        public async Task CreateAsync_NoAccount_Throws()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Category);

            entity.Account = null;

            var result = await this.repository.CreateAsync(entity);

            Assert.IsEmpty(this.context.Transactions.Local);
        }

        [Test]
        [TestCase(TestName = "Create new Transaction with no Category", Category = "Create")]
        public async Task CreateAsync_NoCategory_Throws()
        {
            var entity = this.GenerateObject();

            await this.InsertData(entity.Account);

            entity.Category = null;

            var result = await this.repository.CreateAsync(entity);

            Assert.IsNotEmpty(this.context.Transactions.Local);//TODO: fix
        }
    }
}
