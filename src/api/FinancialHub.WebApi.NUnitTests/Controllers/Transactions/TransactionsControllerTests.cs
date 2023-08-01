using FinancialHub.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        private Random random;
        private TransactionModelBuilder transactionModelBuilder;

        private TransactionsController controller;
        private Mock<ITransactionsService> mockService;
        private Mock<ITransactionBalanceService> mockTransactionBalanceServiceService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.transactionModelBuilder = new TransactionModelBuilder();

            this.mockService = new Mock<ITransactionsService>();
            this.mockTransactionBalanceServiceService = new Mock<ITransactionBalanceService>();
            this.controller = new TransactionsController(this.mockService.Object, this.mockTransactionBalanceServiceService.Object);
        }
    }
}