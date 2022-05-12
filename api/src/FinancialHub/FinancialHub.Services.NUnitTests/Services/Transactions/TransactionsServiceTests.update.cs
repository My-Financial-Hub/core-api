using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidTransactionModel_ReturnsTransactionModel()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once);

            this.mapperWrapper.Verify(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_NonExistingTransactionId_ReturnsResultError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(default(TransactionEntity))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
            Assert.IsTrue(result.HasError);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Never);
        }

        [Test]
        public void UpdateAsync_RepositoryException_ThrowsException()
        {
            var model = this.transactionModelBuilder.Generate();
            var exc = new Exception("mock");

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Throws(exc)
                .Verifiable();

            this.SetUpMapper();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once());
        }
    }
}
