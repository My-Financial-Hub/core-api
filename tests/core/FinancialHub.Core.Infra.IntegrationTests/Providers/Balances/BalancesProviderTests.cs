using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Domain.Interfaces.Providers;
using FinancialHub.Core.Domain.Interfaces.Repositories;

namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    [TestFixtureSource(typeof(FinancialHubInfraFixture))]
    public partial class BalancesProviderTests
    {
        private readonly IBalancesProvider provider;

        private readonly BalanceModelBuilder builder;
        private readonly BalanceEntityBuilder entitybuilder;

        private readonly IBalancesCache cache;
        private readonly IBalancesRepository database;

        private readonly FinancialHubInfraSetup setup;

        public BalancesProviderTests(FinancialHubInfraSetup setup, FinancialHubBuilderSetup builderSetup)
        {
            this.setup = setup;

            this.provider = setup.GetService<IBalancesProvider>();

            this.cache      = setup.GetService<IBalancesCache>();
            this.database  = setup.GetService<IBalancesRepository>();

            this.builder = builderSetup.GetService<BalanceModelBuilder>();
            this.entitybuilder = builderSetup.GetService<BalanceEntityBuilder>();
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.setup.TearUp();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.setup.TearDown();
        }
    }
}
