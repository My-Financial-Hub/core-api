using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;

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

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.transactionDtoBuilder = new TransactionDtoBuilder();
            this.createTransactionDtoBuilder = new CreateTransactionDtoBuilder();
            this.updateTransactionDtoBuilder = new UpdateTransactionDtoBuilder();

            this.mockService = new Mock<ITransactionsService>();
            this.controller = new TransactionsController(this.mockService.Object);
        }
    }
}