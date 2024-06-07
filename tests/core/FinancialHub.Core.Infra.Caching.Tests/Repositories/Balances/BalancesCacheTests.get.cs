using FinancialHub.Core.Infra.Caching.Extensions;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class BalancesCacheTests
    {
        [Test]
        public async Task GetAsync_ExistingBalance_ReturnsBalance()
        {
            var guid = Guid.NewGuid();
            var expectedResult = this.builder.Generate();
            distributedCache
                .Setup(x => x.GetAsync($"balances:{guid}", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult.ToByteArray());

            var balance = await this.cache.GetAsync(guid);

            BalanceModelAssert.Equal(expectedResult, balance);
        }

        [Test]
        public async Task GetAsync_NotExistingBalance_ReturnsNull()
        {
            var guid = Guid.NewGuid();

            var balance = await this.cache.GetAsync(guid);

            Assert.That(balance, Is.Null);
        }
    }
}
