using FinancialHub.Domain.Enums;
using FinancialHub.Services.NUnitTests.Services.TransactionBalance;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FinancialHub.Services.NUnitTests.Services
{
    //TODO: fix UpdateAmount in test names
    //TOOD: maybe separate some testcases
    public partial class TransactionBalanceTests
    {
        class UpdateAmountAsync
        {
            class SameBalance : BaseTransactionBalanceTests
            {
                [TestCase(TransactionStatus.NotCommitted, TransactionType.Expense, true)]
                [TestCase(TransactionStatus.NotCommitted, TransactionType.Earn, true)]
                [TestCase(TransactionStatus.NotCommitted, TransactionType.Expense, false)]
                [TestCase(TransactionStatus.NotCommitted, TransactionType.Earn, false)]
                [TestCase(TransactionStatus.Committed, TransactionType.Expense, false)]
                [TestCase(TransactionStatus.Committed, TransactionType.Earn, false)]
                public async Task NotPaid_DoNotUpdatesAmount(
                    TransactionStatus status, TransactionType type, bool activeStatus
                )
                {
                    var balance = this.balanceModelBuilder.Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(status)
                        .WithType(type)
                        .WithActiveStatus(activeStatus)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(status)
                        .WithType(type)
                        .WithActiveStatus(activeStatus)
                        .Generate();

                    await this.service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never);
                }

                [TestCase(TransactionStatus.Committed, TransactionType.Expense)]
                [TestCase(TransactionStatus.Committed, TransactionType.Earn)]
                public async Task NoChanges_DoNotUpdateAmount(TransactionStatus status, TransactionType type)
                {
                    var balance = this.balanceModelBuilder.Generate();
                    var transactionId = Guid.NewGuid();
                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(status)
                        .WithType(type)
                        .WithActiveStatus(true)
                        .WithId(transactionId)
                        .Generate();

                    await this.service.UpdateAmountAsync(oldTransaction, oldTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Never);
                }

                [TestCase(TransactionType.Expense, TransactionType.Expense)]
                [TestCase(TransactionType.Expense, TransactionType.Earn)]
                [TestCase(TransactionType.Earn, TransactionType.Earn)]
                [TestCase(TransactionType.Earn, TransactionType.Expense)]
                public async Task PaidToNotPaid_RemovesAmountUpdates(TransactionType oldType, TransactionType type)
                {
                    var balanceId = Guid.NewGuid();
                    var balance = this.balanceModelBuilder
                        .WithId(balanceId)
                        .Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.NotCommitted)
                        .WithType(oldType)
                        .WithActiveStatus(true)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(type)
                        .WithActiveStatus(true)
                        .Generate();

                    var expectedResult =
                        type == TransactionType.Earn ?
                        balance.Amount + newTransaction.Amount :
                        balance.Amount - newTransaction.Amount;
                    this.balancesService.Setup(x => x.UpdateAmountAsync(balanceId, expectedResult));

                    await service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(balanceId, expectedResult), Times.Once);
                }

                [TestCase(TransactionType.Expense)]
                [TestCase(TransactionType.Earn)]
                public async Task DifferentAmount_AddsDifference(TransactionType type)
                {
                    var balanceId = Guid.NewGuid();
                    var startValue = random.Next(1000, 10000);
                    var balance = this.balanceModelBuilder
                        .WithAmount(startValue)
                        .WithId(balanceId)
                        .Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(type)
                        .WithActiveStatus(true)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(type)
                        .WithActiveStatus(true)
                        .Generate();
                    var expectedResult =
                        type == TransactionType.Earn ?
                        startValue + newTransaction.Amount - oldTransaction.Amount :
                        startValue + oldTransaction.Amount - newTransaction.Amount;
                    this.balancesService.Setup(x => x.UpdateAmountAsync(balanceId, expectedResult));

                    await service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(balanceId, expectedResult), Times.Once);
                }

                [Test]
                public async Task ExpenseToEarn_AddsRevemovedAmount()
                {
                    var balanceId = Guid.NewGuid();
                    var startValue = random.Next(1000, 10000);
                    var balance = this.balanceModelBuilder
                        .WithAmount(startValue)
                        .WithId(balanceId)
                        .Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(TransactionType.Expense)
                        .WithActiveStatus(true)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(TransactionType.Earn)
                        .WithActiveStatus(true)
                        .Generate();

                    var expectedResult = startValue + (newTransaction.Amount + oldTransaction.Amount);
                    this.balancesService.Setup(x => x.UpdateAmountAsync(balanceId, expectedResult));

                    await service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(balanceId, expectedResult), Times.Once);
                }

                [Test]
                public async Task EarnToExpense_RemovesAddedAmount() {
                    var balanceId = Guid.NewGuid();
                    var startValue = random.Next(1000, 10000);
                    var balance = this.balanceModelBuilder
                        .WithAmount(startValue)
                        .WithId(balanceId)
                        .Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(TransactionType.Earn)
                        .WithActiveStatus(true)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(TransactionType.Expense)
                        .WithActiveStatus(true)
                        .Generate();

                    var expectedResult = startValue - oldTransaction.Amount - newTransaction.Amount;
                    this.balancesService.Setup(x => x.UpdateAmountAsync(balanceId, expectedResult));

                    await service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(balanceId, expectedResult), Times.Once);
                }

                [TestCase(TransactionType.Expense, TransactionType.Expense)]
                [TestCase(TransactionType.Expense, TransactionType.Earn)]
                [TestCase(TransactionType.Earn, TransactionType.Earn)]
                [TestCase(TransactionType.Earn, TransactionType.Expense)]
                public async Task PaidToNotPaid_RemovesAmountChanges(TransactionType oldType, TransactionType type)
                {
                    var balanceId = Guid.NewGuid();
                    var balance = this.balanceModelBuilder
                        .WithId(balanceId)
                        .Generate();

                    var oldTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.Committed)
                        .WithType(oldType)
                        .WithActiveStatus(true)
                        .Generate();

                    var newTransaction = this.transactionModelBuilder
                        .WithBalance(balance)
                        .WithStatus(TransactionStatus.NotCommitted)
                        .WithType(type)
                        .WithActiveStatus(true)
                        .Generate();

                    var expectedResult =
                        type == TransactionType.Earn ?
                        balance.Amount - newTransaction.Amount:
                        balance.Amount + newTransaction.Amount;
                    this.balancesService.Setup(x => x.UpdateAmountAsync(balanceId, expectedResult));

                    await service.UpdateAmountAsync(oldTransaction, newTransaction);

                    this.balancesService.Verify(x => x.UpdateAmountAsync(balanceId, expectedResult), Times.Once);
                }
            }
        }
    }
}
