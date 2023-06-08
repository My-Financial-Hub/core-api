namespace FinancialHub.Auth.IntegrationTests.Base
{
    [TestFixtureSource(typeof(FinancialHubAuthApiFixture))]
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubAuthApiFixture fixture;
        protected HttpClient Client => fixture.Client;

        protected readonly string baseEndpoint;
        protected readonly Random random;

        public BaseControllerTests(FinancialHubAuthApiFixture fixture, string endpoint)
        {
            this.fixture = fixture;
            this.baseEndpoint = endpoint;
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.fixture.CreateDatabase();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.fixture.ClearData();
        }
    }
}
