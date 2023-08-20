using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Application.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        [TestCase(TransactionType.Earn)]
        [TestCase(TransactionType.Expense)]
        public async Task CreateTransactionAsync_PaidTransaction_ShouldReturnCreatedTransaction(TransactionType type)
        {
            var transaction = this.transactionModelBuilder
                .WithType(type)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();
            var balance = this.balanceModelBuilder
                .WithAmount(0)
                .WithId(transaction.BalanceId)
                .Generate();

            this.transactionsService
                .Setup(x => x.CreateAsync(transaction))
                .ReturnsAsync(transaction).Verifiable();
            this.balancesService
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(balance);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, transaction.Amount));

            var result = await this.service.CreateTransactionAsync(transaction);

            Assert.AreEqual(transaction, result.Data);
            Assert.IsFalse(result.HasError);
            Assert.IsNull(result.Error);
        }

        [TestCase(TransactionType.Earn)]
        [TestCase(TransactionType.Expense)]
        public async Task CreateTransactionAsync_PaidTransaction_ShouldUpdateBalance(TransactionType type)
        {
            var transaction = this.transactionModelBuilder
                .WithType(type)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();
            var balance = this.balanceModelBuilder
                .WithId(transaction.BalanceId)
                .Generate();
            var expectedResult = 
                type == TransactionType.Earn? 
                balance.Amount + transaction.Amount:
                balance.Amount - transaction.Amount;

            this.transactionsService
                .Setup(x => x.CreateAsync(transaction))
                .ReturnsAsync(transaction)
                .Verifiable();
            this.balancesService
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedResult))
                .Verifiable();

            await this.service.CreateTransactionAsync(transaction);

            this.transactionsService.Verify(x => x.CreateAsync(transaction), Times.Once);
            this.balancesService.Verify(x => x.GetByIdAsync(transaction.BalanceId), Times.Once);
            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedResult), Times.Once);
        }

        [Test]
        public async Task CreateTransactionAsync_TransactionCreationFailed_ReturnsServiceError()
        {
            var transaction = this.transactionModelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();

            var error = new ServiceError(1, "Error message");
            this.transactionsService
                .Setup(x => x.CreateAsync(transaction))
                .ReturnsAsync(error)
                .Verifiable();

            var result = await this.service.CreateTransactionAsync(transaction);

            Assert.IsNull(result.Data);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(error ,result.Error);
        }

        [Test]
        public async Task CreateTransactionAsync_NotPaidTransaction_ShouldNotUpdateBalance()
        {
            var transaction = this.transactionModelBuilder
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .Generate();

            this.transactionsService
                .Setup(x => x.CreateAsync(transaction))
                .ReturnsAsync(transaction)
                .Verifiable();

            await this.service.CreateTransactionAsync(transaction);

            this.transactionsService.Verify(x => x.CreateAsync(transaction), Times.Once);
            this.balancesService.Verify(x => x.GetByIdAsync(transaction.BalanceId), Times.Never);
            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()), Times.Never);
        }
    }
}
