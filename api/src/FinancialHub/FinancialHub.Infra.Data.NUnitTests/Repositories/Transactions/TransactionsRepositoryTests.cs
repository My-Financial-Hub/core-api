using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Tests.Builders.Entities;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;
using FinancialHub.Infra.Data.Repositories;
using NUnit.Framework;
using System;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class TransactionsRepositoryTests : BaseRepositoryTests<TransactionEntity>
    {
        private AccountEntityBuilder accountEntityBuilder;
        private CategoryEntityBuilder categoryEntityBuilder;

        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new TransactionsRepository(this.context);

            this.accountEntityBuilder = new AccountEntityBuilder();
            this.categoryEntityBuilder = new CategoryEntityBuilder();

            this.builder = new TransactionEntityBuilder();
        }

        protected TransactionEntity GenerateTransaction(Guid? id = null,Guid? accountId = null, Guid? categoryId = null)
        {
            var category = this.GenerateCategory(categoryId);
            var account = this.GenerateAccount(accountId);

            var build = (TransactionEntityBuilder)this.builder;

            if (id == null)
            {
                return build
                    .WithAccount(account)
                    .WithCategory(category)
                    .Generate();
            }
            else
            {
                return build
                    .WithAccount(account)
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

        protected AccountEntity GenerateAccount(Guid? id = null)
        {
            if (id == null)
            {
                return this.accountEntityBuilder.Generate();
            }
            else
            {
                return this.accountEntityBuilder.WithId(id.Value).Generate();
            }
        }
    }
}
