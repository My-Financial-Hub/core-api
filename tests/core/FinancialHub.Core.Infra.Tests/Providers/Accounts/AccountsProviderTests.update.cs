namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task UpdateAsync_ReturnsUpdatedAccount()
        {
            var id = Guid.NewGuid();
            var account = this.accountModelBuilder
                .WithId(id)
                .Generate();

            repository
                .Setup(x => x.UpdateAsync(It.Is<AccountEntity>(x => x.Id == id)))
                .Returns<AccountEntity>(async x => await Task.FromResult(x));

            var result = await provider.UpdateAsync(id, account);

            AccountModelAssert.Equal(account, result);
        }

        [Test]
        public async Task UpdateAsync_CallsAccountsRepository()
        {
            var id = Guid.NewGuid();
            var account = this.accountModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, account);

            repository.Verify(x => x.UpdateAsync(It.IsAny<AccountEntity>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsAccountsRepositoryWithIDFromParam()
        {
            var id = Guid.NewGuid();
            var account = this.accountModelBuilder
                .WithId(id)
                .Generate();

            await provider.UpdateAsync(id, account);
            repository.Verify(x => x.UpdateAsync(It.Is<AccountEntity>(x => x.Id == id)), Times.Once);
        }
    }
}
