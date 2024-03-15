namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        [Test]
        public async Task DeleteAsync_ExistingTransaction_RemovesTransactions()
        {
            var expectedResult = random.Next(1, 100);
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(expectedResult);

            await this.service.DeleteAsync(id);

            this.provider.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ExistingTransaction_ReturnsRemovedTransactionAmount()
        {
            var expectedResult = random.Next(1,100); 
            var id = Guid.NewGuid();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(expectedResult);

            var result = await this.service.DeleteAsync(id);

            Assert.IsInstanceOf<ServiceResult<int>>(result);
            Assert.AreEqual(expectedResult,result.Data);
        }

        [Test]
        public async Task DeleteAsync_NotExistingTransaction_ReturnsNotReturnError()
        {
            var id = Guid.NewGuid();

            var result = await this.service.DeleteAsync(id);

            Assert.Zero(result.Data);
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task DeleteAsync_NotExistingTransaction_DoesNotRemovesTransactions()
        {
            var id = Guid.NewGuid();

            await this.service.DeleteAsync(id);

            this.provider.Verify(x => x.DeleteAsync(id),Times.Once);
        }
    }
}
