using System.Collections;

namespace FinancialHub.Core.Infra.IntegrationTests
{
    [SetUpFixture]
    public class FinancialHubInfraFixture : IEnumerable, IDisposable
    {
        private readonly FinancialHubInfraSetup setup;
        private readonly FinancialHubBuilderSetup builderSetup;

        public FinancialHubInfraFixture() : base()
        {
            setup = new FinancialHubInfraSetup();
            builderSetup = new FinancialHubBuilderSetup();
        }

        [OneTimeSetUp]
        public void OnTimeSetUp()
        {
            this.setup.TearUp();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() 
        { 
            this.setup.TearDown();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerator GetEnumerator()
        {
            yield return new object[] { setup, builderSetup };
        }
    }
}
