using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidTransactionModel_ReturnsTransactionModel()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);

            this.mapperWrapper.Verify(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()),Times.Once);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()),Times.Once);
        }

        [Test]
        public void CreateAsync_RepositoryException_ThrowsException()
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
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Throws(exc)
                .Verifiable();

            this.SetUpMapper();

            var exception = Assert.ThrowsAsync<Exception>(
                async () => await this.service.CreateAsync(model)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once());
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Category with id {model.CategoryId}", result.Error.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidAccount_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error.Message);
        }
    }
}
