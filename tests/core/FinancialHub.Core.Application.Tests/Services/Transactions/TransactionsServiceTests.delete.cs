using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task DeleteAsync_ExistingTransaction_RemovesTransactions()
        {
            var expectedResult = random.Next(1, 100);
            var transaction = this.transactionModelBuilder.Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(expectedResult)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);

            await this.service.DeleteAsync(guid);

            this.provider.Verify(x => x.DeleteAsync(guid), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ExistingTransaction_ReturnsRemovedTransactions()
        {
            var expectedResult = random.Next(1,100);
            var transaction = this.transactionModelBuilder.Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(expectedResult);
            this.provider
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);

            var result = await this.service.DeleteAsync(guid);

            Assert.IsInstanceOf<ServiceResult<int>>(result);
            Assert.AreEqual(expectedResult,result.Data);
        }

        [Test]
        public async Task DeleteAsync_NotExistingTransaction_ReturnsNotFoundError()
        {
            var transaction = this.transactionModelBuilder.Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(guid));

            var result = await this.service.DeleteAsync(guid);

            Assert.Zero(result.Data);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Transaction with id {guid}", result.Error!.Message);
        }

        [Test]
        public async Task DeleteAsync_NotExistingTransaction_DoesNotRemovesTransactions()
        {
            var transaction = this.transactionModelBuilder.Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(guid));

            await this.service.DeleteAsync(guid);

            this.provider.Verify(x => x.GetByIdAsync(guid),Times.Once);
        }

        [TestCase(TransactionStatus.Committed, false)]
        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        public async Task DeleteAsync_ExistingNotCommitedOrInactiveTransaction_RemovesBalanceAmount(
            TransactionStatus status, bool isActive
        )
        {
            var expectedResult = random.Next(1, 100);
            var transaction = this.transactionModelBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(expectedResult);
            this.provider
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);

            await this.service.DeleteAsync(guid);

            this.balancesProvider.Verify(x => x.UpdateAmountAsync(transaction.BalanceId,transaction.Amount), Times.Never);
        }

        [TestCase(TransactionStatus.Committed, true)]
        public async Task DeleteAsync_ExistingCommitedAndActiveTransaction_RemovesBalanceAmount(
            TransactionStatus status, bool isActive
        )
        {
            var expectedResult = random.Next(1, 100);
            var transaction = this.transactionModelBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .Generate();
            var guid = transaction.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(expectedResult);
            this.provider
                .Setup(x => x.GetByIdAsync(guid))
                .ReturnsAsync(transaction);
            
            await this.service.DeleteAsync(guid);

            this.balancesProvider.Verify(x => x.DecreaseAmountAsync(transaction.BalanceId, transaction.Amount, transaction.Type), Times.Once);
        }
    }
}
