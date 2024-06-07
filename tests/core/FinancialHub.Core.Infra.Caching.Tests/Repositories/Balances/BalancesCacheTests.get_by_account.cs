using FinancialHub.Core.Infra.Caching.Extensions;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class BalancesCacheTests
    {
        [Test]
        public async Task GetAsync_ExistingAccount_ReturnsBalances()
        {
            var guid = Guid.NewGuid();
            var expectedResult = this.builder
                .WithAccountId(guid)
                .Generate(10);

            distributedCache
                .Setup(x => x.GetAsync($"accounts:{guid}:balances", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult.ToByteArray());

            var balances = await this.cache.GetByAccountAsync(guid);

            BalanceModelAssert.Equal(expectedResult.ToArray(), balances.ToArray());
        }

        [Test]
        public async Task GetAsync_AccountWithNoBalances_ReturnsEmptyArray()
        {
            var guid = Guid.NewGuid();
            var expectedResult = this.builder
                .WithAccountId(guid)
                .Generate(0);

            distributedCache
                .Setup(x => x.GetAsync($"accounts:{guid}:balances", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult.ToByteArray());

            var balances = await this.cache.GetByAccountAsync(guid);

            Assert.That(balances, Is.Empty);
        }

        [Test]
        public async Task GetAsync_NotExistingAccount_ReturnsNull()
        {
            var guid = Guid.NewGuid();

            var balances = await this.cache.GetByAccountAsync(guid);

            Assert.That(balances, Is.Null);
        }
    }
}
