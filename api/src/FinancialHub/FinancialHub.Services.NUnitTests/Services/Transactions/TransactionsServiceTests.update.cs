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
        [TestCase(Description = "Update valid Transaction", Category = "Update")]
        public async Task UpdateAsync_ValidTransactionModel_ReturnsTransactionModel()
        {
            var model = this.modelGenerator.GenerateTransaction();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>((ent) => this.mapper.Map<TransactionModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>((model) => this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once);

            this.mapperWrapper.Verify(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update non existing Transaction", Category = "Update")]
        public async Task UpdateAsync_NonExistingTransactionId_ReturnsResultError()
        {
            var model = this.modelGenerator.GenerateTransaction();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync((TransactionEntity)null)
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
        [TestCase(Description = "Update repository exception", Category = "Update")]
        public async Task UpdateAsync_RepositoryException_ThrowsException()
        {
            var model = this.modelGenerator.GenerateTransaction();
            var exc = new Exception("mock");

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>((ent) => this.mapper.Map<TransactionModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>((model) => this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once());
        }
    }
}
