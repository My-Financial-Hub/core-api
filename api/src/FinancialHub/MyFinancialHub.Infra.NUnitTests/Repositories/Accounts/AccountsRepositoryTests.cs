using System;
using NUnit.Framework;
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

        protected override AccountEntity GenerateObject(Guid? id = null)
        {

            return this.generator.GenerateAccount(id);
        }
    }
}
