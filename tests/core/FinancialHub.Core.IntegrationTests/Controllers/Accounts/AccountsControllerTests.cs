namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class AccountsControllerTests : BaseControllerTests
    {
        private AccountEntityBuilder accountBuilder;
        private BalanceEntityBuilder balanceBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public AccountsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/accounts")
        {

        }

        public override void SetUp()
        {
            accountBuilder = new AccountEntityBuilder();
            balanceBuilder = new BalanceEntityBuilder();
            transactionBuilder = new TransactionEntityBuilder();
            AddCreateAccountBuilder();
            AddUpdateAccountBuilder();
            base.SetUp();
        }

        protected async Task AssertGetExists(AccountModel expected)
        {
            var getResponse = await client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            AccountModelAssert.Equal(expected, getResult!.Data.First());
        }

        protected void Populate(int amount = 10)
        {
            var account = accountBuilder.Generate();
            var data = balanceBuilder.WithAccountId(account.Id).Generate(amount);
            fixture.AddData(account);
            fixture.AddData(data.ToArray());
        }
    }
}