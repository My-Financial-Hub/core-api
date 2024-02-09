namespace FinancialHub.Core.IntegrationTests.Controllers.Balances
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task Post_BalanceWithInvalidAccount_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var data = modelBuilder.WithAccountId(id).Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Account with id {id}");
        }

        [Test]
        public async Task Post_BalanceWithInvalidAccount_DoesNotCreateBalance()
        {
            var id = Guid.NewGuid();
            var data = modelBuilder.WithAccountId(id).Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var balance = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.AccountId == id);
            Assert.IsNull(balance);
        }

        [Test]
        public async Task Post_ValidBalance_ReturnsCreatedBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var data = modelBuilder.WithAccountId(account.Id).Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.IsNotNull(result?.Data);
            BalanceModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidBalance_CreatesBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = modelBuilder.WithAccountId(account.Id).Generate();

            await client.PostAsync(baseEndpoint, body);

            AssertExists(body);
        }
        
        [Test]
        public async Task Post_ValidBalance_CreatesBalanceWithAmountZero()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);
            var body = modelBuilder.WithAccountId(account.Id).Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.Zero(result!.Data.Amount);
        }
    }
}
