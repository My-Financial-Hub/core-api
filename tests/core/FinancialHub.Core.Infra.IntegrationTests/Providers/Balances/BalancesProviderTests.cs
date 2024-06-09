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

        private readonly IAccountsRepository accountsRepository;

        public BalancesProviderTests(FinancialHubInfraSetup setup, FinancialHubBuilderSetup builderSetup)
        {
            this.provider = setup.GetService<IBalancesProvider>();

            this.cache      = setup.GetService<IBalancesCache>();
            this.database   = setup.GetService<IBalancesRepository>();

            this.accountsRepository = setup.GetService<IAccountsRepository>();

            this.builder = builderSetup.GetService<BalanceModelBuilder>();
            this.entitybuilder = builderSetup.GetService<BalanceEntityBuilder>();
        }
    }
}
