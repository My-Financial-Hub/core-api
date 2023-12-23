namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class BalancesProviderTests
    {
        [Test]
        public async Task CreateAsync_ReturnsCreatedBalance()
        {
            var balance = this.balanceModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async x => await Task.FromResult(x));

            var result = await this.provider.CreateAsync(balance);

            Assert.That(result, Is.Not.Null);
            BalanceModelAssert.Equal(balance, result);
        }

        [Test]
        public async Task CreateAsync_CallsBalanceRepository()
        {
            var balance = this.balanceModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(balance);

            repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }
    }
}
