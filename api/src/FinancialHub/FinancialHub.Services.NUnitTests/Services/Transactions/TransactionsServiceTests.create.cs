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
        [TestCase(Description = "Create valid Transaction", Category = "Create")]
        public async Task CreateAsync_ValidTransactionModel_ReturnsTransactionModel()
        {
            var model = this.transactionModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
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

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);

            this.mapperWrapper.Verify(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()),Times.Once);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()),Times.Once);
        }

        [Test]
        [TestCase(Description = "Create repository exception", Category = "Create")]
        public void CreateAsync_RepositoryException_ThrowsException()
        {
            var model = this.transactionModelBuilder.Generate();
            var exc = new Exception("mock");

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
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
                async () => await this.service.CreateAsync(model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once());
        }
    }
}
