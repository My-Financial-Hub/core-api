using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class TransactionsControllerTests
    {
        private Random random;

        private TransactionDtoBuilder transactionDtoBuilder;
        private CreateTransactionDtoBuilder createTransactionDtoBuilder;
        private UpdateTransactionDtoBuilder updateTransactionDtoBuilder;

        private TransactionsController controller;
        private Mock<ITransactionsService> mockService;
        private Mock<ILogger<TransactionsController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.transactionDtoBuilder = new TransactionDtoBuilder();
            this.createTransactionDtoBuilder = new CreateTransactionDtoBuilder();
            this.updateTransactionDtoBuilder = new UpdateTransactionDtoBuilder();

            this.mockService = new Mock<ITransactionsService>();
            this.mockLogger = new Mock<ILogger<TransactionsController>>();
            this.controller = new TransactionsController(this.mockService.Object, this.mockLogger.Object);
        }
    }
}