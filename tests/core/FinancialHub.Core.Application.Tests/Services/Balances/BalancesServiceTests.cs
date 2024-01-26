using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Providers;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        protected Random random;
        protected BalanceModelBuilder balanceModelBuilder;

        private IBalancesService service;

        private Mock<IBalancesProvider> provider;
        private Mock<IAccountsProvider> accountsProvider;
        private Mock<IErrorMessageProvider> errorMessageProvider;

        [SetUp]
        public void Setup()
        {
            this.provider         = new Mock<IBalancesProvider>();
            this.accountsProvider = new Mock<IAccountsProvider>();
            this.errorMessageProvider = new Mock<IErrorMessageProvider>();
            this.service            = new BalancesService(
                provider.Object, 
                accountsProvider.Object,
                errorMessageProvider.Object
            );

            this.random = new Random();

            this.balanceModelBuilder = new BalanceModelBuilder();
        }
    }
}
