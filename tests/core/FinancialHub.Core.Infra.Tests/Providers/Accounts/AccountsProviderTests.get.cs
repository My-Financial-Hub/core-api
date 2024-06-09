namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task GetByIdAsync_ExistingAccount_ReturnsAccount()
        {
            var id = Guid.NewGuid();
            var accountEntity = accountEntityBuilder
                .WithId(id)
                    .Generate();
            var expectedAccount = mapper.Map<AccountModel>(accountEntity);

            repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(accountEntity);

            var result = await provider.GetByIdAsync(id);

            AccountModelAssert.Equal(expectedAccount, result);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingAccount_ReturnsNull()
        {
            var id = Guid.NewGuid();

            var result = await provider.GetByIdAsync(id);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_CachedAccount_ReturnsAccountFromCache()
        {
            var id = Guid.NewGuid();
            var account = accountModelBuilder
                .WithId(id)
                .Generate();

            cache
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(account);

            var result = await provider.GetByIdAsync(id);

            AccountModelAssert.Equal(account, result);
        }
    }
}
