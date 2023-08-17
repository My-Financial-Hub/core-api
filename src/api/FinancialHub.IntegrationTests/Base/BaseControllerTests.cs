using System.Net.Http;
using FinancialHub.IntegrationTests.Setup;

namespace FinancialHub.IntegrationTests.Base
{
    [TestFixtureSource(typeof(FinancialHubApiFixture))]
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        protected readonly string baseEndpoint;
        protected readonly Random random;

        protected BaseControllerTests(FinancialHubApiFixture fixture,string endpoint)
        {
            this.fixture = fixture;
            this.baseEndpoint = endpoint;
            this.random = new Random();
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
