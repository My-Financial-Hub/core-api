namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAccountList()
        {
            var accountEntities = accountEntityBuilder.Generate(random.Next(0, 10));
            var expectedAccounts = mapper.Map<IEnumerable<AccountModel>>(accountEntities);

            repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(accountEntities);

            var accounts = await provider.GetAllAsync();

            AccountModelAssert.Equal(expectedAccounts.ToArray(), accounts.ToArray());
        }
    }
}
