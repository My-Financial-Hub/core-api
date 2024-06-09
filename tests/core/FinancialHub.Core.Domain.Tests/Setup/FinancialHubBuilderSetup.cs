using FinancialHub.Core.Domain.Tests.Setup.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialHub.Core.Domain.Tests.Setup
{
    public class FinancialHubBuilderSetup : FinancialHubSetup
    {
        public FinancialHubBuilderSetup()
        {
            this.services
                .AddModelBuilders()
                .AddEntityBuilders();

            this.serviceProvider = this.services.BuildServiceProvider();
        }
        //TODO: fix this inherit structure

        public override void TearDown()
        {
            throw new NotImplementedException();
        }

        public override void TearUp()
        {
            throw new NotImplementedException();
        }
    }
}
