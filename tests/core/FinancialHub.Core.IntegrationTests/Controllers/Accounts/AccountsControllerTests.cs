using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.IntegrationTests.Controllers.Accounts
{
    public partial class AccountsControllerTests : BaseControllerTests
    {
        private AccountEntityBuilder entityBuilder;
        private AccountModelBuilder modelBuilder;
        private BalanceEntityBuilder balanceBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public AccountsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/accounts")
        {

        }

        public override void SetUp()
        {
            modelBuilder = new AccountModelBuilder();
            entityBuilder = new AccountEntityBuilder();
            balanceBuilder = new BalanceEntityBuilder();
            transactionBuilder = new TransactionEntityBuilder();
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
            var account = entityBuilder.Generate();
            var data = balanceBuilder.WithAccountId(account.Id).Generate(amount);
            fixture.AddData(account);
            fixture.AddData(data.ToArray());
        }
    }
}