using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;
using static FinancialHub.Common.Results.Errors.ValidationError;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        private UpdateTransactionDtoBuilder updateTransactionDtoBuilder;
        protected void AddUpdateTransactionBuilder()
        {
            updateTransactionDtoBuilder = new UpdateTransactionDtoBuilder();
        }

        [Test]
        public async Task UpdateAsync_ValidTransaction_UpdatesTransaction()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()))
                .Returns<Guid, TransactionModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.UpdateAsync(id, updateTransaction);
            
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidTransaction_ReturnsTransaction()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()))
                .Returns<Guid, TransactionModel>(async (_, x) => await Task.FromResult(x));

            var result = await this.service.UpdateAsync(id, updateTransaction);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<TransactionDto>>(result);
        }

        [Test]
        public async Task UpdateAsync_InvalidTransaction_DoNotUpdatesTransaction()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(new ValidationError("Transaction Validation Error", Array.Empty<FieldValidationError>()));

            await this.service.UpdateAsync(id, updateTransaction);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_InvalidTransaction_ReturnsValidationError()
        {
            var expectedMessage = "Transaction Validation Error";
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<FieldValidationError>()));

            var result = await this.service.UpdateAsync(id, updateTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_NonExistingTransactionId_ReturnsNotFoundError()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();
            var expectedMessage = $"Not found transaction with id {id}";

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedMessage));

            var result = await this.service.UpdateAsync(id, updateTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_NonExistingTransactionId_DoNotUpdatesTransaction()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError($"Not found transaction with id {id}"));

            await this.service.UpdateAsync(id, updateTransaction);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();
            var expectedErrorMessage = $"Not found Category with id {updateTransaction.CategoryId}";

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.UpdateAsync(id, updateTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidCategory_DoNotUpdatesTransaction()
        {
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(ServiceResult.Success);
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(new NotFoundError($"Not found Category with id {updateTransaction.CategoryId}"));
            
            await this.service.UpdateAsync(id, updateTransaction);
            
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var updateTransaction = updateTransactionDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Balance with id {updateTransaction.BalanceId}";

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);

            var result = await this.service.UpdateAsync(id, updateTransaction);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_DoNotUpdatesTransaction()
        {
            var id = Guid.NewGuid();
            var updateTransaction = updateTransactionDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateTransaction))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.balancesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.BalanceId))
                .ReturnsAsync(new NotFoundError($"Not found Balance with id {updateTransaction.BalanceId}"));
            this.categoriesValidator
                .Setup(x => x.ExistsAsync(updateTransaction.CategoryId))
                .ReturnsAsync(ServiceResult.Success);

            var result = await this.service.UpdateAsync(id, updateTransaction);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Never);
        }
    }
}
