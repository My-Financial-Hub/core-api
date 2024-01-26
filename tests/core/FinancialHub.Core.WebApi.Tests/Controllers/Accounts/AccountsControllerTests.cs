using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class AccountsControllerTests
    {
        private Random random;

        private AccountModelBuilder accountModelBuilder;

        private AccountsController controller;
        private Mock<IAccountsService> mockService;
        private Mock<IBalancesService> mockBalanceService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();

            this.mockService = new Mock<IAccountsService>();
            this.mockBalanceService = new Mock<IBalancesService>();
            this.controller = new AccountsController(this.mockService.Object, this.mockBalanceService.Object);
        }
    }
}
