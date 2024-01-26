namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task DeleteAsync_RemovesBalance()
        {
            var expectedResult = random.Next(1, 100);
            var guid = Guid.NewGuid();
            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .Returns(async () => await Task.FromResult(expectedResult))
                .Verifiable();

            await this.service.DeleteAsync(guid);

            this.provider.Verify(x => x.DeleteAsync(guid), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_RepositorySuccess_ReturnsRemovedBalances()
        {
            var expectedResult = random.Next(1,100);
            var guid = Guid.NewGuid();
            this.provider
                .Setup(x => x.DeleteAsync(guid))
                .Returns(async () => await Task.FromResult(expectedResult))
                .Verifiable();

            var result = await this.service.DeleteAsync(guid);

            Assert.IsInstanceOf<ServiceResult<int>>(result);
            Assert.AreEqual(expectedResult,result.Data);
        }
    }
}
