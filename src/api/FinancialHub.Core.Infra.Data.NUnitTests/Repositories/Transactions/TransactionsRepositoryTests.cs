using FinancialHub.Core.Infra.Data.Repositories;
using FinancialHub.Core.Infra.Data.NUnitTests.Repositories.Base;

namespace FinancialHub.Core.Infra.Data.NUnitTests.Repositories
{
    public partial class TransactionsRepositoryTests : BaseRepositoryTests<TransactionEntity>
    {
        private BalanceEntityBuilder balanceEntityBuilder;
        private CategoryEntityBuilder categoryEntityBuilder;

        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new TransactionsRepository(this.context);

            this.balanceEntityBuilder = new BalanceEntityBuilder();
            this.categoryEntityBuilder = new CategoryEntityBuilder();

            this.builder = new TransactionEntityBuilder();
        }

        protected TransactionEntity GenerateTransaction(Guid? id = null,Guid? accountId = null, Guid? categoryId = null)
        {
            var category = this.GenerateCategory(categoryId);
            var balance = this.GenerateBalance(accountId);

            var build = (TransactionEntityBuilder)this.builder;

            if (id == null)
            {
                return build
                    .WithBalance(balance)
                    .WithCategory(category)
                    .Generate();
            }
            else
            {
                return build
                    .WithBalance(balance)
                    .WithCategory(category)
                    .WithId(id.Value)
                    .Generate();
            }
        }

        protected CategoryEntity GenerateCategory(Guid? id = null)
        {
            if (id == null)
            {
                return this.categoryEntityBuilder.Generate();
            }
            else
            {
                return this.categoryEntityBuilder.WithId(id.Value).Generate();
            }
        }

        protected BalanceEntity GenerateBalance(Guid? id = null)
        {
            if (id == null)
            {
                return this.balanceEntityBuilder.Generate();
            }
            else
            {
                return this.balanceEntityBuilder.WithId(id.Value).Generate();
            }
        }
    }
}
