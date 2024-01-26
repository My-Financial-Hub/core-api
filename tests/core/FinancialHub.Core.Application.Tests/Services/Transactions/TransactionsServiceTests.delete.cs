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
            var id = transaction.Id.GetValueOrDefault();
            var expectedErrorMessage = $"Not found Transaction with id {id}";

            this.provider.Setup(x => x.GetByIdAsync(id));
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var result = await this.service.DeleteAsync(id);

            Assert.Zero(result.Data);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
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
    }
}
