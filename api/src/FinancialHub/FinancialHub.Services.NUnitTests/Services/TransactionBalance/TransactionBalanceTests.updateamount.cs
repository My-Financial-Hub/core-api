using FinancialHub.Domain.Enums;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        [Test]
        public async Task UpdateAmountAsync_EarnPaid_IncreasesValue()
        {
            var balance = this.balanceModelBuilder
                .Generate();

            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();

            var expectedAmount = balance.Amount + transaction.Amount;

            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .ReturnsAsync(balance)
                .Verifiable();

            await this.service.UpdateAmountAsync(transaction, balance);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }

        [Test]
        public async Task UpdateAmountAsync_UndoEarn_DecreasesValue()
        {
            var balance = this.balanceModelBuilder
                .Generate();

            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .Generate();

            balance.Amount += transaction.Amount;

            var expectedAmount = balance.Amount - transaction.Amount;

            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .ReturnsAsync(balance)
                .Verifiable();

            await this.service.UpdateAmountAsync(transaction, balance, undo: true);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }

        [Test]
        public async Task UpdateAmountAsync_ExpensePaid_DecreasesValue()
        {
            var balance = this.balanceModelBuilder
                .Generate();

            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();

            balance.Amount += transaction.Amount;

            var expectedAmount = balance.Amount - transaction.Amount;

            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .ReturnsAsync(balance)
                .Verifiable();

            await this.service.UpdateAmountAsync(transaction, balance);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }

        [Test]
        public async Task UpdateAmountAsync_UndoExpense_IncreasesValue()
        {
            var balance = this.balanceModelBuilder
                .Generate();

            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.NotCommitted)
                .Generate();

            var expectedAmount = balance.Amount + transaction.Amount;

            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .ReturnsAsync(balance)
                .Verifiable();

            await this.service.UpdateAmountAsync(transaction, balance, true);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount), Times.Once);
        }

        [TestCase(TransactionType.Expense)]
        [TestCase(TransactionType.Earn)]
        public async Task UpdateAmountAsync_UndoAndNotPaid_DoNotUpdateValue(TransactionType type)
        {
            var balance = this.balanceModelBuilder
                .Generate();

            var transaction = this.transactionModelBuilder
                .WithBalance(balance)
                .WithType(type)
                .WithStatus(TransactionStatus.NotCommitted)
                .Generate();

            var expectedAmount = balance.Amount + transaction.Amount;

            this.balancesService
                .Setup(x => x.UpdateAmountAsync(transaction.BalanceId, expectedAmount))
                .ReturnsAsync(balance)
                .Verifiable();

            await this.service.UpdateAmountAsync(transaction, balance, false);

            this.balancesService.Verify(x => x.UpdateAmountAsync(transaction.BalanceId, It.IsAny<decimal>()), Times.Never);
        }
    }
}
