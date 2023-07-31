using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FinancialHub.Common.Results.Errors;
using FinancialHub.Domain.Enums;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionBalanceTests
    {
        [Test]
        public async Task UpdateTransactionAsync_ValidTransactionWithDifferentBalances_UpdatesBalancesAmount()
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

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);

            this.balancesService
                .Setup(x => x.GetByIdAsync(oldTransaction.BalanceId))
                .ReturnsAsync(oldBalance);
            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(newBalance);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, It.IsAny<decimal>()))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            Assert.IsFalse(result.HasError);
            Assert.AreSame(newTransaction, result.Data);
            this.balancesService
                .Verify(
                    x => x.UpdateAmountAsync(It.IsAny<Guid>(), It.IsAny<decimal>()),
                    Times.Between(1,2, Moq.Range.Inclusive)
                 );
        }

        [Test]
        public async Task UpdateTransactionAsync_ValidTransactionWithSameBalance_UpdatesBalanceAmount()
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

            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(oldTransaction);
            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(newTransaction);

            this.balancesService
                .Setup(x => x.GetByIdAsync(newTransaction.BalanceId))
                .ReturnsAsync(balance);
            this.balancesService
                .Setup(x => x.UpdateAmountAsync(newTransaction.BalanceId, It.IsAny<decimal>()))
                .Verifiable();

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            Assert.IsFalse(result.HasError);
            Assert.AreSame(newTransaction, result.Data);
            this.balancesService
                .Verify(
                    x => x.UpdateAmountAsync(newTransaction.BalanceId, It.IsAny<decimal>()),
                    Times.Once
                 );
        }

        [Test]
        public async Task UpdateTransactionAsync_TransactionUpdateFailed_ReturnsUpdateError()
        {
            var balance = this.balanceModelBuilder.Generate();
            var id = Guid.NewGuid();
            var newTransaction = this.transactionModelBuilder
                .WithType(TransactionType.Earn)
                .WithStatus(TransactionStatus.NotCommitted)
                .WithActiveStatus(true)
                .WithBalance(balance)
                .WithId(id)
                .Generate();

            var error = new ServiceError(1, "Update error message");
            this.transactionsService
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(newTransaction);

            this.transactionsService
                .Setup(x => x.UpdateAsync(id, newTransaction))
                .ReturnsAsync(error);

            var result = await this.service.UpdateTransactionAsync(id, newTransaction);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(error.Message, result.Error.Message);
        }
    }
}
