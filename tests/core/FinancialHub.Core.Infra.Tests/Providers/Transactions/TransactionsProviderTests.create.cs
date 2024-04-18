using FinancialHub.Core.Domain.Enums;
using Moq;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        [Test]
        public async Task CreateAsync_CallsTransactionRepository()
        {
            var transaction = this.transactionModelBuilder.Generate();
            
            repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));
            balancesProvider
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.CreateAsync(transaction);

            repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var transaction = this.transactionModelBuilder.Generate();

            balancesProvider
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);
            repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));

            var result = await this.provider.CreateAsync(transaction);

            Assert.That(result, Is.Not.Null);
            TransactionModelAssert.Equal(transaction, result);
        }

        [Test]
        public async Task CreateAsync_EarnPaidTransaction_IncreasesAmount()
        {
            var transaction = this.transactionModelBuilder.Generate();
            var balanceAmount = transaction.Balance.Amount;

            balancesProvider
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);
            await this.provider.CreateAsync(transaction);

            balancesProvider.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, balanceAmount + transaction.Amount), Times.Once());
        }

        [Test]
        public async Task CreateAsync_ExpensesPaidTransaction_DecreasesAmount()
        {
            var transaction = this.transactionModelBuilder.Generate();
            var balanceAmount = transaction.Balance.Amount;

            balancesProvider
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.CreateAsync(transaction);

            balancesProvider.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, balanceAmount + transaction.Amount), Times.Once());
        }

        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, false)]
        public async Task CreateAsync_NotPaidTransaction_DoesNotUpdateAmount(TransactionStatus status, bool isActive)
        {
            var transaction = this.transactionModelBuilder.Generate();

            balancesProvider
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.CreateAsync(transaction);

            balancesProvider.Verify(x => x.UpdateAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never());
        }
    }
}
