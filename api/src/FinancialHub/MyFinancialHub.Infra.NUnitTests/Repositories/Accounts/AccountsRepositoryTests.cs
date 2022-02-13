using System;
using NUnit.Framework;
using FinancialHub.Domain.Entities;
using FinancialHub.Infra.Data.Repositories;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories.Accounts
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
