namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task CreateAsync_ReturnsCreatedAccount()
        {
            var account = accountModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            var result = await this.provider.CreateAsync(account);

            AccountModelAssert.Equal(account, result);
        }

        [Test]
        public async Task CreateAsync_CallsAccountsRepository()
        {
            var account = accountModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(account);

            repository.Verify(x => x.CreateAsync(It.IsAny<AccountEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CallsAccountsCache()
        {
            var account = accountModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(account);

            cache.Verify(x => x.AddAsync(It.IsAny<AccountModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CallsCreateBalancesRepository()
        {
            var account = accountModelBuilder.Generate();

            repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(account);

            balancesProvider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CallsBalanceRepository()
        {
            var account = accountModelBuilder.Generate();
            repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            await this.provider.CreateAsync(account);

            balancesProvider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Once);
        }
    }
}
