namespace FinancialHub.Core.Infra.IntegrationTests.Providers
{
    public partial class BalancesProviderTests
    {
        [Test]
        public async Task CreateAsync_AddsToDatabase()
        {
            var balanceCount = database.Balances.Count();
            var balance = this.builder.Generate();

            await this.provider.CreateAsync(balance);

            Assert.Multiple(() =>
            {
                Assert.That(database.Balances.Count(), Is.EqualTo(balanceCount + 1));
                Assert.That(
                    database.Balances.FirstOrDefault(x =>
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
        public async Task CreateAsync_NoAccountInCache_AddsBalanceToId()
        {
            var account = this.entitybuilder.Generate().Account;
            await this.database.AddAsync(account);
            await this.database.SaveChangesAsync();

            var balance = this.builder
                .WithAccountId(account.Id)
                .Generate();

            await this.provider.CreateAsync(balance);
           
            Assert.Fail();
        }

        [Test]
        public async Task CreateAsync_ExistingAccountInCache_UpdatesBalances()
        {
            var account = this.entitybuilder.Generate().Account;
            await this.database.AddAsync(account);
            await this.database.SaveChangesAsync();

            var balance = this.builder
                .WithAccountId(account.Id)
                .Generate();

            await this.provider.CreateAsync(balance);

            Assert.Fail();
        }

        [Test]
        public async Task CreateAsync_NoAccount_DoNotAddBalanceToId()
        {
            var balance = this.builder.Generate();

            await this.provider.CreateAsync(balance);
        
            Assert.Fail();
        }

        [Test]
        public async Task CreateAsync_NoAccount_DoNotUpdateBalances()
        {
            var balance = this.builder.Generate();

            await this.provider.CreateAsync(balance);
        
            Assert.Fail();
        }
    }
}
