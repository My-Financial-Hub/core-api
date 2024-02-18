using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task GetAccountsBalances_ReturnsBalances()
        {
            var expectedAmount = random.Next(1, 10);
            Populate(random.Next(1, 10));

            var account = accountBuilder.Generate();
            var data = balanceBuilder.WithAccountId(account.Id).Generate(expectedAmount);
            fixture.AddData(account);
            fixture.AddData(data.ToArray());

            var response = await client.GetAsync($"{baseEndpoint}/{account.Id}/balances");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<BalanceDto>>();
            Assert.AreEqual(expectedAmount, result?.Data.Count);
        }

        [Test]
        public async Task GetAccountsBalances_AccountWithNoBalances_ReturnsEmptyList()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var response = await client.GetAsync($"{baseEndpoint}/{account.Id}/balances");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<BalanceDto>>();
            Assert.AreEqual(0, result?.Data.Count);
        }

        [Test]
        public async Task GetAccountsBalances_NoAccount_Returns404NotFound()
        {
            var expectedAmount = random.Next(1, 10);
            Populate(random.Next(1, 10));

            var account = accountBuilder.Generate();
            var data = balanceBuilder.WithAccountId(account.Id).Generate(expectedAmount);
            fixture.AddData(account);
            fixture.AddData(data.ToArray());

            var response = await client.GetAsync($"{baseEndpoint}/{Guid.NewGuid()}/balances");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
