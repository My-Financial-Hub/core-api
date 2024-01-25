using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        [Test]
        public async Task DeleteAsync_ReturnsAmountOfLinesDeleted()
        {
            var deletedLines = 1;
            var guid = Guid.NewGuid();

            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(deletedLines);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(deletedLines);

            var result = await this.provider.DeleteAsync(guid);

            Assert.That(result, Is.EqualTo(deletedLines));
        }

        [Test]
        public async Task DeleteAsync_PaidTransaction_UpdatesAmount()
        {
            var guid = Guid.NewGuid();

            var transaction = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithId(guid)
                .Generate();

            repository
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);
            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(1);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);
            balanceRepository
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.DeleteAsync(guid);

            balanceRepository.Verify(x => x.ChangeAmountAsync(transaction.BalanceId, It.IsAny<decimal>()), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_EarnPaidTransaction_DecreasesAmount()
        {
            var guid = Guid.NewGuid();

            var transaction = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Earn)
                .WithActiveStatus(true)
                .WithId(guid)
                .Generate();
            var balanceAmount = transaction.Balance.Amount;

            repository
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);
            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(1);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);
            balanceRepository
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.DeleteAsync(guid);

            balanceRepository.Verify(x => x.ChangeAmountAsync(transaction.BalanceId, balanceAmount - transaction.Amount), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_ExpensesPaidTransaction_IncreasesAmount()
        {
            var guid = Guid.NewGuid();

            var transaction = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Expense)
                .WithActiveStatus(true)
                .WithId(guid)
                .Generate();
            var balanceAmount = transaction.Balance.Amount;

            repository
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);
            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(1);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);
            balanceRepository
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.DeleteAsync(guid);

            balanceRepository.Verify(x => x.ChangeAmountAsync(transaction.BalanceId, balanceAmount + transaction.Amount), Times.Once());
        }

        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, false)]
        public async Task DeleteAsync_NotPaidTransaction_DoNotUpdateAmount(TransactionStatus status, bool isActive)
        {
            var guid = Guid.NewGuid();

            var transaction = this.transactionBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .WithId(guid)
                .Generate();

            repository
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);
            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(1);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);
            balanceRepository
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(transaction.Balance);

            await this.provider.DeleteAsync(guid);

            balanceRepository.Verify(x => x.ChangeAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never());
        }
    }
}
