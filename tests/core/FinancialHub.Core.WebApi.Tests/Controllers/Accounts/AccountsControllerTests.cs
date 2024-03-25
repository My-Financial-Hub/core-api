using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class AccountsControllerTests
    {
        private Random random;

        private AccountDtoBuilder accountDtoBuilder;
        private CreateAccountDtoBuilder createAccountDtoBuilder;
        private UpdateAccountDtoBuilder updateAccountDtoBuilder;

        private AccountsController controller;
        private Mock<IAccountsService> mockService;
        private Mock<IBalancesService> mockBalanceService;
        private Mock<ILogger<AccountsController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();

            this.accountDtoBuilder = new AccountDtoBuilder();
            this.createAccountDtoBuilder = new CreateAccountDtoBuilder();
            this.updateAccountDtoBuilder = new UpdateAccountDtoBuilder();

            this.mockService = new Mock<IAccountsService>();
            this.mockBalanceService = new Mock<IBalancesService>();
            this.mockLogger = new Mock<ILogger<AccountsController>>();
            this.controller = new AccountsController(this.mockService.Object, this.mockBalanceService.Object, mockLogger.Object);
        }
    }
}
