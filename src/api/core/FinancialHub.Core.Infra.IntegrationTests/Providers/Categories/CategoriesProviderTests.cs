using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    [TestFixtureSource(typeof(FinancialHubInfraFixture))]
    public partial class CategoriesProviderTests
    {
        private readonly ICategoriesProvider provider;

        private readonly IDistributedCache cache;
        private readonly FinancialHubContext database;

        private readonly CategoryModelBuilder builder;
        private readonly CategoryEntityBuilder entitybuilder;

        public CategoriesProviderTests(FinancialHubInfraSetup setup, FinancialHubBuilderSetup builderSetup)
        {
            this.provider = setup.GetService<ICategoriesProvider>();

            this.cache = setup.GetService<IDistributedCache>();
            this.database = setup.GetService<FinancialHubContext>();

            this.builder = builderSetup.GetService<CategoryModelBuilder>();
            this.entitybuilder = builderSetup.GetService<CategoryEntityBuilder>();
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
