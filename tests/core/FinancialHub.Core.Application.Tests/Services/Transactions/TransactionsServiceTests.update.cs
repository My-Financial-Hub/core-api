﻿namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidTransaction_UpdatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.categoriesProvider
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category)
                .Verifiable();
            this.balancesProvider
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()))
                .Returns<Guid, TransactionModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidTransaction_ReturnsTransaction()
        {
            var model = this.transactionModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.categoriesProvider.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category)
                .Verifiable();
            this.balancesProvider.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()))
                .Returns<Guid, TransactionModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
        }

        [Test]
        public async Task UpdateAsync_NonExistingTransactionId_ReturnsResultError()
        {
            var model = this.transactionModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(TransactionModel))
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<TransactionModel>()))
                .Returns<Guid,TransactionModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
            Assert.IsTrue(result.HasError);
        }

        [Test]
        public async Task UpdateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();
            var expectedErrorMessage = $"Not found Category with id {model.CategoryId}";

            this.balancesProvider.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(model.Balance)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(model)
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();
            var expectedErrorMessage = $"Not found Balance with id {model.BalanceId}";
            this.categoriesProvider.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(model.Category)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(model)
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }
    }
}
