using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Enums;
using System;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidTransaction_UpdatesTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance))
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [TestCase(TransactionStatus.NotCommitted, true,  TransactionStatus.Committed, true)]
        [TestCase(TransactionStatus.Committed,    false, TransactionStatus.Committed, true)]
        [TestCase(TransactionStatus.NotCommitted, false, TransactionStatus.Committed, true)]
        public async Task UpdateAsync_CommitedAndActiveTransaction_AddsBalance(
            TransactionStatus oldStatus, bool oldIsActive,
            TransactionStatus newStatus, bool newIsActive)
        {
            var oldModel = this.transactionBuilder
                .WithStatus(oldStatus)
                .WithActiveStatus(oldIsActive)
                .Generate();
            var model = this.transactionModelBuilder
                .WithStatus(newStatus)
                .WithActiveStatus(newIsActive)
                .Generate();

            var balance = this.mapper.Map<BalanceEntity>(model.Balance);

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(oldModel)
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);
            this.balancesRepository.Verify(x => x.ChangeAmountAsync(balance.Id.GetValueOrDefault(),model.Amount, model.Type,false));
            //this.balancesRepository.Verify(x => x.AddAmountAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [TestCase(TransactionStatus.Committed, true, TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.Committed, true, TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, true, TransactionStatus.Committed,    false)]
        public async Task UpdateAsync_NotCommitedOrInactiveTransaction_RemovesBalance(
            TransactionStatus oldStatus, bool oldIsActive,
            TransactionStatus newStatus, bool newIsActive)
        {
            var oldModel = this.transactionBuilder
                .WithStatus(oldStatus)
                .WithActiveStatus(oldIsActive)
                .Generate();
            var model = this.transactionModelBuilder
                .WithStatus(newStatus)
                .WithActiveStatus(newIsActive)
                .Generate();

            var balance = this.mapper.Map<BalanceEntity>(model.Balance);

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(oldModel)
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);
            this.balancesRepository.Verify(x => x.ChangeAmountAsync(balance.Id.GetValueOrDefault(),model.Amount, model.Type,true));
            //this.balancesRepository.Verify(x => x.RemoveAmountAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [TestCase(TransactionStatus.NotCommitted, true,  TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, true,  TransactionStatus.Committed,    false)]
        [TestCase(TransactionStatus.NotCommitted, true,  TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.NotCommitted, false, TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false, TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.NotCommitted, false, TransactionStatus.Committed,    false)]
        [TestCase(TransactionStatus.Committed,    false, TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.Committed,    false, TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed,    false, TransactionStatus.Committed,    false)]
        public async Task UpdateAsync_NoStatusOrTypeChanges_DoesNotUpdate(
            TransactionStatus oldStatus, bool oldIsActive,
            TransactionStatus newStatus, bool newIsActive)
        {
            var oldModel = this.transactionBuilder
                .WithStatus(oldStatus)
                .WithActiveStatus(oldIsActive)
                .Generate();
            var model = this.transactionModelBuilder
                .WithStatus(newStatus)
                .WithActiveStatus(newIsActive)
                .Generate();

            var balance = this.mapper.Map<BalanceEntity>(model.Balance);

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(oldModel)
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);
            this.balancesRepository.Verify(x => x.ChangeAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<TransactionType>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_DifferentAmounts_ChangesBalance()
        {
            var oldAmount = random.Next(1, 10);
            var oldModel = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithAmount(oldAmount)
                .Generate();
            var balance = this.mapper.Map<BalanceEntity>(oldModel.Balance);

            var newAmount = random.Next(11, 100);
            var model = this.transactionModelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalanceId(balance.Id)
                .WithAmount(newAmount)
                .Generate();

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(oldModel)
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            this.balancesRepository.Verify(x => x.ChangeAmountAsync(model.BalanceId, oldAmount - newAmount, model.Type, false), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_BalanceChanges_MoveAmountsFromOldBalanceToNewBalance()
        {
            var oldAmount = random.Next(1,10);
            var oldModel = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithAmount(oldAmount)
                .Generate();
            var oldBalance = this.mapper.Map<BalanceEntity>(oldModel.Balance);

            var newAmount = random.Next(11,100);
            var model = this.transactionModelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithAmount(newAmount)
                .Generate();
            var balance = this.mapper.Map<BalanceEntity>(model.Balance);

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(oldModel.BalanceId))
                .ReturnsAsync(oldBalance)
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(oldModel)
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            this.balancesRepository.Verify(x => x.ChangeAmountAsync(oldModel.BalanceId, oldAmount, oldModel.Type, true), Times.Once);
            this.balancesRepository.Verify(x => x.ChangeAmountAsync(model.BalanceId, newAmount, model.Type, false),Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidTransaction_ReturnsTransaction()
        {
            var model = this.transactionModelBuilder.Generate();

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance))
                .Verifiable();

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
        }

        [Test]
        public async Task UpdateAsync_InvalidCategory_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.SetUpMapper();

            this.balancesRepository.Setup(x => x.GetByIdAsync(model.BalanceId))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model.Balance))
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Category with id {model.CategoryId}", result.Error.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var model = this.transactionModelBuilder.Generate();

            this.SetUpMapper();

            this.categoriesRepository.Setup(x => x.GetByIdAsync(model.CategoryId))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model.Category))
                .Verifiable();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<TransactionEntity>(model))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.BalanceId}", result.Error.Message);
        }
    }
}
