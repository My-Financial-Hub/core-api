using NUnit.Framework;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FinancialHub.IntegrationTests.Setup;
using System.Text;
using System;

namespace FinancialHub.IntegrationTests.Base
{
    public abstract class BaseControllerTests
    {
        protected readonly FinancialHubApiFixture fixture;
        protected HttpClient client => fixture.Client;

        public BaseControllerTests()
        {
            this.fixture = new FinancialHubApiFixture();
        }

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.fixture.ClearData();
        }
    }
}
