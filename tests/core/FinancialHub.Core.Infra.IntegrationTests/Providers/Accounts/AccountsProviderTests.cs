using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    [TestFixtureSource(typeof(FinancialHubInfraFixture))]
    public partial class AccountsProviderTests
    {
        private readonly IAccountsProvider provider;

        private readonly AccountModelBuilder builder;
        private readonly AccountEntityBuilder entitybuilder;

        private readonly FinancialHubContext database;
        private readonly IDistributedCache cache;

        public AccountsProviderTests(FinancialHubInfraSetup setup, FinancialHubBuilderSetup builderSetup)
        {
            this.provider       = setup.GetService<IAccountsProvider>();

            this.cache          = setup.GetService<IDistributedCache>();
            this.database       = setup.GetService<FinancialHubContext>();

            this.builder        = builderSetup.GetService<AccountModelBuilder>();
            this.entitybuilder  = builderSetup.GetService<AccountEntityBuilder>();
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
