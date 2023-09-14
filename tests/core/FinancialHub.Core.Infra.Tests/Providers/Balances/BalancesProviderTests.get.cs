namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class BalancesProviderTests
    {
        [Test]
        public async Task GetAllByAccountAsync_ExistingAccount_ReturnsBalanceList()
        {
            var id = Guid.NewGuid();
            var accountEntity = accountEntityBuilder
                .WithId(id)
                .Generate();
            var balanceEntities = balanceEntityBuilder
                .WithAccount(accountEntity)
                .Generate(random.Next(0, 10));
            var expectedAccounts = mapper.Map<IEnumerable<BalanceModel>>(balanceEntities);

            repository
                .Setup(x => x.GetAsync(It.IsAny<Func<BalanceEntity, bool>>()))
                .ReturnsAsync(balanceEntities);

            var accounts = await provider.GetAllByAccountAsync(accountEntity.Id.GetValueOrDefault());

            BalanceModelAssert.Equal(expectedAccounts.ToArray(), accounts.ToArray());
        }

        [Test]
        public async Task GetByIdAsync_ExistingBalance_ReturnsBalance()
        {
            var id = Guid.NewGuid();
            var balanceEntity = balanceEntityBuilder
                .WithId(id)
                .Generate();
            var expectedAccount = mapper.Map<BalanceModel>(balanceEntity);

            repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(balanceEntity);

            var result = await provider.GetByIdAsync(id);

            BalanceModelAssert.Equal(expectedAccount, result!);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingBalance_ReturnsNull()
        {
            var id = Guid.NewGuid();

            var result = await provider.GetByIdAsync(id);

            Assert.That(result, Is.Null);
        }
    }
}
