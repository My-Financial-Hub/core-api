using FinancialHub.Domain.Entities;
using FinancialHub.Infra.NUnitTests.Repositories.Base;
using FinancialHub.Infra.Repositories;
using NUnit.Framework;
using System;

namespace FinancialHub.Infra.NUnitTests.Repositories.Transactions
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
