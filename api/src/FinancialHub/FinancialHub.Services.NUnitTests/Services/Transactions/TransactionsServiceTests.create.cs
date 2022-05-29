using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidTransaction_CreatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.CreateAsync(model);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransactionModel_UpdatesBalance()
        {
            var model = this.transactionModelBuilder.Generate();
            var balanceEntity = this.mapper.Map<BalanceEntity>(model.Balance);

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balanceEntity);

            this.balancesRepository
                .Setup(x => x.UpdateAsync(balanceEntity))
                .ReturnsAsync(balanceEntity);

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.CreateAsync(model);

            this.balancesRepository.Verify(x => x.UpdateAsync(balanceEntity), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<TransactionModel>>(result);
        }

        [Test]
        public void CreateAsync_RepositoryException_ThrowsException()
        {
            var model = this.transactionModelBuilder.Generate();
            var exc = new Exception("mock");

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

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

            this.SetUpMapper();

            this.balancesRepository
                .Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance));

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Category with id {model.CategoryId}", result.Error.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.SetUpMapper();

            this.categoriesRepository
                .Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category));

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.BalanceId}", result.Error.Message);
        }
    }
}
