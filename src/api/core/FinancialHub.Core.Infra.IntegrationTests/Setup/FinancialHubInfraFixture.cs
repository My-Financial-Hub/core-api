using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using FinancialHub.Core.Infra.Extensions;
using FinancialHub.Core.Infra.Data.Extensions.Configurations;
using FinancialHub.Core.Infra.Caching.Extensions.Configurations;
using FinancialHub.Core.Domain.Tests.Setup;

namespace FinancialHub.Core.Infra.IntegrationTests.Setup
{
    public class FinancialHubInfraFixture : FinancialHubFixture, IEnumerable, IDisposable
    {
        public FinancialHubInfraFixture() : base()
        {
            services
                .AddCoreInfra()
                .AddCaching(configuration)
                .AddRepositories(configuration)
                .AddLogging();

            serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerator GetEnumerator()
        {
            yield return this;
        }
    }
}
