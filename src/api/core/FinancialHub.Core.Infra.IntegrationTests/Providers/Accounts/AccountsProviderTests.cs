using Microsoft.Extensions.Caching.Distributed;
using FinancialHub.Core.Infra.IntegrationTests.Setup;
using FinancialHub.Core.Domain.Interfaces.Providers;
using FinancialHub.Core.Infra.Data.Contexts;
using FinancialHub.Core.Domain.Tests.Setup;
using FinancialHub.Core.Domain.Tests.Builders.Models;

namespace FinancialHub.Core.Infra.IntegrationTests.Providers.Accounts
{
    [TestFixtureSource(typeof(FinancialHubInfraFixture))]
    public partial class AccountsProviderTests
    {
        private readonly IAccountsProvider provider;

        private readonly AccountModelBuilder builder;

        private readonly FinancialHubContext database;
        private readonly IDistributedCache cache;

        public AccountsProviderTests(FinancialHubInfraSetup setup, FinancialHubBuilderSetup builderSetup)
        {
            this.provider = setup.GetService<IAccountsProvider>();

            this.cache = setup.GetService<IDistributedCache>();
            this.database = setup.GetService<FinancialHubContext>();
            this.builder = builderSetup.GetService<AccountModelBuilder>();
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.database.Database.EnsureCreated();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.database.Database.EnsureDeleted();
        }
    }
}
