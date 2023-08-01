using FinancialHub.Core.Infra.Data.Repositories;
using FinancialHub.Core.Infra.Data.NUnitTests.Repositories.Base;

namespace FinancialHub.Core.Infra.Data.NUnitTests.Repositories
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
