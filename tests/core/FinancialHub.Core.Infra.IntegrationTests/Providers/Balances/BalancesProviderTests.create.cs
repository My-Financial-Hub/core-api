namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    public partial class BalancesProviderTests
    {
        [Test]
        public async Task CreateAsync_AddsToDatabase()
        {
            var balanceCount = (await database.GetAllAsync()).Count;
            var balance = this.builder.Generate();

            await this.provider.CreateAsync(balance);

            Assert.Multiple(async () =>
            {
                var balances = await database.GetAllAsync();
                Assert.That(balances, Has.Count.EqualTo(balanceCount + 1));
                Assert.That(
                    balances.FirstOrDefault(x =>
                        x.Name == balance.Name &&
                        x.AccountId == balance.AccountId &&
                        x.Currency == balance.Currency &&
                        x.IsActive == balance.IsActive
                    ),
                    Is.Not.Null
                );
            });
        }

        [Test]
        public async Task CreateAsync_NoBalanceInCache_AddsBalanceToId()
        {
            var account = this.entitybuilder.Generate().Account;
            await this.accountsRepository.CreateAsync(account);
            await this.accountsRepository.CommitAsync();

            var balance = this.builder
                .WithAccountId(account.Id)
                .Generate();

            var createdBalance = await this.provider.CreateAsync(balance);

            Assert.Multiple(async () =>
            {
                var cacheBalance = await this.cache.GetAsync(createdBalance.Id.GetValueOrDefault());
                BalanceModelAssert.Equal(balance, cacheBalance);
            });
        }

        [Test]
        public async Task CreateAsync_ExistingAccountInCache_UpdatesBalancelist()
        {
            var account = this.entitybuilder.Generate().Account;
            await this.accountsRepository.CreateAsync(account);
            await this.accountsRepository.CommitAsync();

            var balanceCount = new Random().Next(3,10);
            var balances = this.builder
                .WithAccountId(account.Id)
                .Generate(balanceCount);
            await this.cache.AddAsync(balances);

            var balance = this.builder
                .WithAccountId(account.Id)
                .Generate();

            await this.provider.CreateAsync(balance);

            Assert.Multiple(async () =>
            {
                var cacheBalances = await this.cache.GetByAccountAsync(account.Id.GetValueOrDefault());
                Assert.That(cacheBalances, Has.Count.EqualTo(balanceCount + 1));
                BalanceModelAssert.Equal(balances.Append(balance).ToArray(), cacheBalances.ToArray());
            });
        }
    }
}
