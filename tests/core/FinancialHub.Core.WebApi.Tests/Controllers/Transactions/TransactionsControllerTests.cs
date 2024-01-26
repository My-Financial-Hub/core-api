using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class TransactionsControllerTests
    {
        private Random random;
        private TransactionModelBuilder transactionModelBuilder;

        private TransactionsController controller;
        private Mock<ITransactionsService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.transactionModelBuilder = new TransactionModelBuilder();

            this.mockService = new Mock<ITransactionsService>();
            this.controller = new TransactionsController(this.mockService.Object);
        }
    }
}