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
    }
}
