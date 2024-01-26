using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        [Test]
        public async Task CreateAsync_CallsTransactionRepository()
        {
            var transactionEntity = this.transactionBuilder
                .Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>(async x => await Task.FromResult(x));
            balanceRepository
                .Setup(x => x.GetByIdAsync(transactionEntity.BalanceId))
                .ReturnsAsync(transactionEntity.Balance);

            var transaction = this.transactionModelBuilder
                .WithStatus(transactionEntity.Status)
                .WithType(transactionEntity.Type)
                .WithActiveStatus(transactionEntity.IsActive)
                .WithBalanceId(transactionEntity.BalanceId)
                .WithAmount(transactionEntity.Amount)
                .WithId(transactionEntity.Id!.Value)
                .Generate();
            await this.provider.CreateAsync(transaction);

            repository.Verify(x => x.CreateAsync(It.IsAny<TransactionEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidTransaction_ReturnsCreatedTransaction()
        {
            var transaction = this.transactionModelBuilder.Generate();

            var balance = balanceBuilder
                .WithId(transaction.BalanceId)
                .Generate();

            balanceRepository
                .Setup(x => x.GetByIdAsync(transaction.BalanceId))
                .ReturnsAsync(balance);
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
            var transactionEntity = this.transactionBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Earn)
                .WithActiveStatus(true)
                .Generate();
            var balanceAmount = transactionEntity.Balance.Amount;

            balanceRepository
                .Setup(x => x.GetByIdAsync(transactionEntity.BalanceId))
                .ReturnsAsync(transactionEntity.Balance);

            var transaction = this.transactionModelBuilder
                .WithStatus(transactionEntity.Status)
                .WithType(transactionEntity.Type)
                .WithActiveStatus(transactionEntity.IsActive)
                .WithBalanceId(transactionEntity.BalanceId)
                .WithAmount(transactionEntity.Amount)
                .WithId(transactionEntity.Id!.Value)
                .Generate();
            await this.provider.CreateAsync(transaction);

            balanceRepository.Verify(x => x.ChangeAmountAsync(transactionEntity.BalanceId, balanceAmount + transaction.Amount), Times.Once());
        }

        [Test]
        public async Task CreateAsync_ExpensesPaidTransaction_DecreasesAmount()
        {
            var transactionEntity = this.transactionBuilder
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();
            var balanceAmount = transactionEntity.Balance.Amount;

            balanceRepository
                .Setup(x => x.GetByIdAsync(transactionEntity.BalanceId))
                .ReturnsAsync(transactionEntity.Balance);

            var transaction = this.transactionModelBuilder
                .WithStatus(transactionEntity.Status)
                .WithType(transactionEntity.Type)
                .WithActiveStatus(transactionEntity.IsActive)
                .WithBalanceId(transactionEntity.BalanceId)
                .WithAmount(transactionEntity.Amount)
                .WithId(transactionEntity.Id!.Value)
                .Generate();
            await this.provider.CreateAsync(transaction);

            balanceRepository.Verify(x => x.ChangeAmountAsync(transaction.BalanceId, balanceAmount - transaction.Amount), Times.Once());
        }

        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, false)]
        public async Task CreateAsync_NotPaidTransaction_DoesNotUpdateAmount(TransactionStatus status, bool isActive)
        {
            var transactionEntity = this.transactionBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .Generate();

            balanceRepository
                .Setup(x => x.GetByIdAsync(transactionEntity.BalanceId))
                .ReturnsAsync(transactionEntity.Balance);

            var transaction = this.transactionModelBuilder
                .WithStatus(transactionEntity.Status)
                .WithType(transactionEntity.Type)
                .WithActiveStatus(transactionEntity.IsActive)
                .WithBalanceId(transactionEntity.BalanceId)
                .WithAmount(transactionEntity.Amount)
                .WithId(transactionEntity.Id!.Value)
                .Generate();
            await this.provider.CreateAsync(transaction);

            balanceRepository.Verify(x => x.ChangeAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never());
        }
    }
}
