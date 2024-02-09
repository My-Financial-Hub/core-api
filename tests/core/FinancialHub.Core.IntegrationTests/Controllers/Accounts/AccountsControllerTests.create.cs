namespace FinancialHub.Core.IntegrationTests.Controllers.Accounts
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task Post_ValidAccount_ReturnCreatedAccount()
        {
            var data = modelBuilder.Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            AccountModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidAccount_CreateAccount()
        {
            var body = modelBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            await AssertGetExists(body);
        }

        [Test]
        public async Task Post_ValidAccount_CreateDefaultBalance()
        {
            var body = modelBuilder.Generate();

            await client.PostAsync(baseEndpoint, body);

            var account = fixture.GetData<AccountEntity>().First();
            var balances = fixture.GetData<BalanceEntity>();

            Assert.IsNotNull(balances.FirstOrDefault(x => x.AccountId == account.Id && x.Name == $"{account.Name} Default Balance"));
        }
    }
}
