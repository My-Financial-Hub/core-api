using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        private CreateTransactionDtoBuilder createTransactionDtoBuilder;
        protected void AddCreateTransactionBuilder()
        {
            createTransactionDtoBuilder = new CreateTransactionDtoBuilder();
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_CreatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var createTransaction = createTransactionDtoBuilder
                .WithBalanceId(model.BalanceId)
                .WithCategoryId(model.CategoryId)
                .Generate();
            await this.service.CreateAsync(createTransaction);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x))
                .Verifiable();
            
            var createTransaction = createTransactionDtoBuilder
                .WithBalanceId(model.BalanceId)
                .WithCategoryId(model.CategoryId)
                .Generate();
            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionDto>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();
            var expectedErrorMessage = $"Not found Category with id {model.CategoryId}";

            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance);
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var createTransaction = createTransactionDtoBuilder
                .WithBalanceId(model.BalanceId)
                .WithCategoryId(model.CategoryId)
                .Generate();
            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();
            var expectedErrorMessage = $"Not found Balance with id {model.BalanceId}";
            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category);
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var createTransaction = createTransactionDtoBuilder
                .WithBalanceId(model.BalanceId)
                .WithCategoryId(model.CategoryId)
                .Generate();
            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }
    }
}
