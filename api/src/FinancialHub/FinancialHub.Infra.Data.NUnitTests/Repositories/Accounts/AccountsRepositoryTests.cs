using System;
using NUnit.Framework;
using FinancialHub.Domain.Entities;
using FinancialHub.Infra.Data.Repositories;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;
using FinancialHub.Domain.Tests.Builders.Entities;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public class AccountsRepositoryTests : BaseRepositoryTests<AccountEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new AccountsRepository(this.context);
            this.builder = new AccountEntityBuilder();
        }
    }
}
