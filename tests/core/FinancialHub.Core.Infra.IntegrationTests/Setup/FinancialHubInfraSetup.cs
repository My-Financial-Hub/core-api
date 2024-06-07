using FinancialHub.Core.Infra.Extensions;
using FinancialHub.Core.Infra.Data.Extensions.Configurations;
using FinancialHub.Core.Infra.Caching.Extensions.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Infra.IntegrationTests.Setup
{
    public class FinancialHubInfraSetup : FinancialHubSetup
    {
        public FinancialHubInfraSetup() : base()
        {
            services
                .AddCoreInfra()
                .AddCaching(configuration)
                .AddRepositories(configuration)
                .AddLogging();

            serviceProvider = services.BuildServiceProvider();
        }

        public override void TearDown()
        {
            this.GetService<FinancialHubContext>().Database.EnsureCreated();
        }

        public override void TearUp()
        {
            this.GetService<FinancialHubContext>().Database.EnsureDeleted();
        }
    }
}
