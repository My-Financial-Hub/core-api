using System.Net.Http;
using NUnit.Framework;
using FinancialHub.IntegrationTests.Setup;

namespace FinancialHub.IntegrationTests.Base
{
    [TestFixtureSource(typeof(FinancialHubApiFixture))]
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        protected readonly string baseEndpoint;

        public BaseControllerTests(FinancialHubApiFixture fixture,string endpoint)
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
