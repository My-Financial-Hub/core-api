using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;
using Moq;
using static FinancialHub.Common.Results.Errors.ValidationError;

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
            var createTransaction = createTransactionDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(createTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.CreateAsync(createTransaction);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(createTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>(async (x) => await Task.FromResult(x));

            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionDto>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidTransaction_ReturnsNotFoundError()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = "Transaction Error";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(new ValidationError(expectedErrorMessage, Array.Empty<FieldValidationError>()));
            
            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidTransaction_DoNotCreatesTransaction()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = "Transaction Error";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(new ValidationError(expectedErrorMessage, Array.Empty<FieldValidationError>()));

            await this.service.CreateAsync(createTransaction);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Category with id {createTransaction.CategoryId}";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(createTransaction.CategoryId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_DoNotCreatesTransaction()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Category with id {createTransaction.CategoryId}";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(createTransaction.CategoryId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            await this.service.CreateAsync(createTransaction);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Balance with id {createTransaction.BalanceId}";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));
            var result = await this.service.CreateAsync(createTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_DoNotCreatesTransaction()
        {
            var createTransaction = createTransactionDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Balance with id {createTransaction.BalanceId}";

            this.validator
                .Setup(x => x.ValidateAsync(createTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(createTransaction.BalanceId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            await this.service.CreateAsync(createTransaction);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<TransactionModel>()), Times.Never);
        }
    }
}
