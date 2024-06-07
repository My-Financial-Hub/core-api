using FinancialHub.Core.Infra.Caching.Extensions;

namespace FinancialHub.Core.Infra.Caching.Tests.Repositories
{
    public partial class AccountsCacheTests
    {
        [Test]
        public async Task GetAsync_ExistingAccount_ReturnsAccount()
        {
            var guid = Guid.NewGuid();
            var expectedResult = this.builder.Generate();
            distributedCache
                .Setup(x => x.GetAsync($"accounts:{guid}",It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult.ToByteArray());

            var account = await this.cache.GetAsync(guid);

            AccountModelAssert.Equal(expectedResult, account);
        }

        [Test]
        public async Task GetAsync_NotExistingCategory_ReturnsNull()
        {
            var guid = Guid.NewGuid();

            var account = await this.cache.GetAsync(guid);

            Assert.That(account, Is.Null);
        }
    }
}
