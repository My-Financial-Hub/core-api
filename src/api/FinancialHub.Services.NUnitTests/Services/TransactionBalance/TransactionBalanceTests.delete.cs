using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        [Test]
        public async Task DeleteTransactionAsync_ValidTransactionId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder.WithId(id).Generate();
            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(1);
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()));

            var result = await this.service.DeleteTransactionAsync(id);

            Assert.IsFalse(result.HasError);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public async Task DeleteTransactionAsync_NoDeletion_ReturnsFalse()
        {
            var id = Guid.NewGuid();
            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(0);

            var result = await this.service.DeleteTransactionAsync(id);
            Assert.IsFalse(result.Data);
        }

        [Test]
        public async Task DeleteTransactionAsync_TransactionDeleteError_ReturnsError()
        {
            var id = Guid.NewGuid(); 
            var error = new ServiceError(1, "error message");
            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(error);
            var result = await this.service.DeleteTransactionAsync(id);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(error.Message ,result.Error.Message);
        }

        [Test]
        public async Task DeleteTransactionAsync_PaidTransaction_UpdatesAmount()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithId(id)
                .Generate();

            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(1);
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()))
                .Verifiable();
            await this.service.DeleteTransactionAsync(id);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()), Times.Once);
        }

        [Test]
        public async Task DeleteTransactionAsync_NotPaidTransaction_DoNotUpdateAmount()
        {
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithStatus(TransactionStatus.NotCommitted)
                .WithId(id)
                .Generate();

            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(1);
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);
            await this.service.DeleteTransactionAsync(id);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()), Times.Never);
        }

        [Test]
        public async Task DeleteTransactionAsync_EarnTransction_RemovesAmount()
        {
            var balance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithId(id)
                .Generate();
            var expectedAmount = balance.Amount - transaction.Amount;

            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(1);
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .Verifiable();
            await this.service.DeleteTransactionAsync(id);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }

        [Test]
        public async Task DeleteTransactionAsync_ExpenseTransction_AddsAmount()
        {
            var balance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithId(id)
                .Generate();
            var expectedAmount = balance.Amount + transaction.Amount;

            this.transactionsService
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(1);
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(transaction);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .Verifiable();
            await this.service.DeleteTransactionAsync(id);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }
    }
}
