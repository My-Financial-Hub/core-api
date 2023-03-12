using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        public static object TestCases {
            get
            {
                var balance1Id = Guid.NewGuid();
                var balance2Id = Guid.NewGuid();
                return new object[1]
                {
                    new object[8]
                    {
                        TransactionStatus.Committed, TransactionType.Expense, balance1Id, 10,
                        TransactionStatus.Committed, TransactionType.Expense, balance2Id, 10
                    }
                };
            }
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnPaidToEarnNotPaidSameBalance_RemovesAmountFromBalance()
        {
            var balance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();
            var expectedValue = balance.Amount - oldTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnPaidToExpensePaidSameBalance_RemovesAmountFromBalance()
        {
            var id = Guid.NewGuid();
            var balance = this.balanceModelBuilder
                .Generate();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithAmount(oldTransaction.Amount)
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();

            var expectedValue = balance.Amount - oldTransaction.Amount - newTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnPaidToExpenseNotPaidSameBalance_RemovesAmountFromBalance()
        {
            var id = Guid.NewGuid();
            var balance = this.balanceModelBuilder
                .Generate();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithAmount(oldTransaction.Amount)
                .WithType(TransactionType.Expense)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();

            var expectedValue = balance.Amount - oldTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnNotPaidToEarnPaidSameBalance_AddsDifferenceToSameBalance()
        {
            var balance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();

            var expectedValue = balance.Amount + oldTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(balance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, expectedValue), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnPaidToNotEarnPaidDifferentBalance_RemovesAmountFromFirstBalance()
        {
            var oldBalance = this.balanceModelBuilder.Generate();
            var newBalance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(oldBalance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(newBalance)
                .WithId(id)
                .Generate();

            var oldExpectedValue = oldBalance.Amount - oldTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(oldBalance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(oldTransaction.BalanceId, oldExpectedValue))
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, It.IsAny<decimal>()))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(oldTransaction.BalanceId, oldExpectedValue), Times.Once);
            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, It.IsAny<decimal>()), Times.Never);
        }

        [Test]
        public async Task UpdateTransactionAsync_EarnNotPaidToEarnPaidDifferentBalance_AddsAmountToSecondBalance()
        {
            var oldBalance = this.balanceModelBuilder.Generate();
            var newBalance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var oldTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .WithBalance(oldBalance)
                .WithId(id)
                .Generate();
            var newTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(newBalance)
                .WithId(id)
                .Generate();

            var newExpectedValue = newBalance.Amount + newTransaction.Amount;

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(oldBalance)
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(oldTransaction.BalanceId, It.IsAny<decimal>()))
                .Verifiable();
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, newExpectedValue))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            this.balancesService.Verify(x => x.UpdateAmountAsync(oldTransaction.BalanceId, It.IsAny<decimal>()), Times.Never);
            this.balancesService.Verify(x => x.UpdateAmountAsync(newTransaction.BalanceId, newExpectedValue), Times.Once);
        }
    }
}
