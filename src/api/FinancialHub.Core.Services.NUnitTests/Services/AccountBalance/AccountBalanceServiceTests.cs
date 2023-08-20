using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Application.Services;

namespace FinancialHub.Core.Application.NUnitTests.Services
{
    public partial class AccountBalanceServiceTests
    {
        protected Random random;
        protected AccountModelBuilder accountModelBuilder;
        protected BalanceModelBuilder balanceModelBuilder;

        private IAccountBalanceService service;

        private Mock<IBalancesService> balanceService;
        private Mock<IAccountsService> accountsService;

        [SetUp]
        public void Setup()
        {
            this.balanceService = new Mock<IBalancesService>();
            this.accountsService = new Mock<IAccountsService>();
            this.service = new AccountBalanceService(
                balanceService.Object,
                accountsService.Object
            );

            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();
            this.balanceModelBuilder = new BalanceModelBuilder();
        }
    }
}
