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

        protected override TransactionEntity GenerateObject(Guid? id = null, params object[] props)
        {
            var len = props.Length;
            var active = random.Next(0, 1);

            var accountId = len > 5 ? new Guid(props[4].ToString()) : Guid.NewGuid();
            var categoryId = len > 6 ? new Guid(props[5].ToString()) : Guid.NewGuid();
            
            return new TransactionEntity()
            {
                Id = id ?? Guid.NewGuid(),
                Description = len > 1 ? props[0].ToString() : Guid.NewGuid().ToString(),
                Amount = len > 2 ? (double)props[1] : random.Next(0, 100000),
                TargetDate = len > 3 ? (DateTimeOffset)props[2] : DateTimeOffset.UtcNow,
                FinishDate = len > 4 ? (DateTimeOffset)props[3] : DateTimeOffset.UtcNow,

                AccountId = accountId,
                Account = this.GenerateAccount(accountId),

                CategoryId = categoryId,
                Category = this.GenerateCategory(categoryId),

                IsActive = len > 7 ? (bool)props[6] : active == 1,
                Status = len > 8 ? (int)props[7] : random.Next(0, 4),
                Type = len > 9 ? (int)props[8] : random.Next(0, 1),
                CreationTime = DateTimeOffset.UtcNow,
                UpdateTime = DateTimeOffset.UtcNow,
            };
        }

        protected CategoryEntity GenerateCategory(Guid? id = null, string name = null)
        {
            return new CategoryEntity()
            {
                Id = id ?? Guid.NewGuid(),
                Name = name ?? Guid.NewGuid().ToString()
            };
        }

        protected AccountEntity GenerateAccount(Guid? id = null, string name = null)
        {
            return new AccountEntity()
            {
                Id = id ?? Guid.NewGuid(),
                Name = name ?? Guid.NewGuid().ToString()
            };
        }
    }
}
