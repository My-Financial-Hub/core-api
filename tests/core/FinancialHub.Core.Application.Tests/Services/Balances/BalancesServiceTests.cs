using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        protected Random random;
        protected BalanceModelBuilder balanceModelBuilder;

        private IBalancesService service;

        private Mock<IBalancesProvider> provider;
        private Mock<IAccountsProvider> accountsProvider;

        [SetUp]
        public void Setup()
        {
            this.provider         = new Mock<IBalancesProvider>();
            this.accountsProvider = new Mock<IAccountsProvider>();
            this.service            = new BalancesService(
                provider.Object, 
                accountsProvider.Object
            );

            this.random = new Random();

            this.balanceModelBuilder = new BalanceModelBuilder();
        }
    }
}
