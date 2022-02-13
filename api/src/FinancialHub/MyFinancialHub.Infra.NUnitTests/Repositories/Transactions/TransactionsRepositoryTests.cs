using FinancialHub.Domain.Entities;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;
using FinancialHub.Infra.Data.Repositories;
using NUnit.Framework;
using System;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories.Transactions
{
    public partial class TransactionsRepositoryTests : BaseRepositoryTests<TransactionEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new TransactionsRepository(this.context);
        }

        protected override TransactionEntity GenerateObject(Guid? id = null)
        {
            return this.generator.GenerateTransaction(id);
        }

        protected TransactionEntity GenerateTransaction(Guid? id = null,Guid? accountId = null, Guid? categoryId = null)
        {
            return this.generator.GenerateTransaction(id,accountId,categoryId);
        }

        protected CategoryEntity GenerateCategory(Guid? id = null)
        {
            return this.generator.GenerateCategory(id);
        }

        protected AccountEntity GenerateAccount(Guid? id = null)
        {
            return this.generator.GenerateAccount(id);
        }
    }
}
