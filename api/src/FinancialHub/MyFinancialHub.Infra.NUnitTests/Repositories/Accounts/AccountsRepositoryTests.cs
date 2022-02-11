using System;
using NUnit.Framework;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;
using FinancialHub.Infra.Repositories;
using FinancialHub.Infra.NUnitTests.Repositories.Base;

namespace FinancialHub.Infra.NUnitTests.Repositories.Accounts
{
    public class AccountsRepositoryTests : BaseRepositoryTests<AccountEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new AccountsRepository(this.context);
        }

        protected override ICollection<AccountEntity> GenerateData(int min = 1, int max = 100, params object[] props)
        {
            var count = random.Next(min,max);
            var accounts = new AccountEntity[count];

            for (int i = 0; i < count; i++)
            {
                accounts[i] = this.GenerateObject(Guid.NewGuid(), props);
            }

            return accounts;
        }

        protected override AccountEntity GenerateObject(Guid? id = null, params object[] props)
        {
            var active = random.Next(0,1);
            
            return new AccountEntity()
            {
                Id              = id,
                Name            = props.Length > 0 ? props[0].ToString() : Guid.NewGuid().ToString(),
                Description     = props.Length > 1 ? props[1].ToString() : Guid.NewGuid().ToString(),
                Currency        = props.Length > 2 ? props[2].ToString() : Guid.NewGuid().ToString(),
                IsActive        = props.Length > 3 ? (bool)props[3]      : active == 1,
                CreationTime    = DateTimeOffset.UtcNow,
                UpdateTime      = DateTimeOffset.UtcNow,
            };
        }
    }
}
