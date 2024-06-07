using System.Collections;

namespace FinancialHub.Core.Infra.IntegrationTests.Setup
{
    public class FinancialHubInfraFixture : IEnumerable, IDisposable
    {
        private readonly FinancialHubInfraSetup setup;
        private readonly FinancialHubBuilderSetup builderSetup;

        public FinancialHubInfraFixture() : base()
        {
            setup = new FinancialHubInfraSetup();
            builderSetup = new FinancialHubBuilderSetup();
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
