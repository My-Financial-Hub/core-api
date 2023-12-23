namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class BalancesProviderTests
    {
        [Test]
        public async Task UpdateAsync_ReturnsUpdatedBalance()
        {
            var id = Guid.NewGuid();
            var balance = this.balanceModelBuilder
                .WithId(id)
                .Generate();

            repository
                .Setup(x => x.UpdateAsync(It.Is<BalanceEntity>(x => x.Id == id)))
                .Returns<BalanceEntity>(async x => await Task.FromResult(x));

            var result = await provider.UpdateAsync(id, balance);

            BalanceModelAssert.Equal(balance, result);
        }

        [Test]
        public async Task UpdateAsync_CallsBalancesRepository()
        {
            var id = Guid.NewGuid();
            var balance = this.balanceModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, balance);

            repository.Verify(x => x.UpdateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsBalancesRepositoryWithIDFromParam()
        {
            var id = Guid.NewGuid();
            var balance = this.balanceModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, balance);
            repository.Verify(x => x.UpdateAsync(It.Is<BalanceEntity>(x => x.Id == id)), Times.Once);
        }
    }
}
