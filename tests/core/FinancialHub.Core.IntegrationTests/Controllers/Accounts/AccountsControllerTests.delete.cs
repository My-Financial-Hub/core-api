namespace FinancialHub.Core.IntegrationTests.Controllers.Accounts
{
    public partial class AccountsControllerTests
    {
        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            fixture.AddData(data);

            var response = await client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesAccountFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            fixture.AddData(data);

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            Assert.IsEmpty(fixture.GetData<AccountEntity>());
        }

        [Test]
        public async Task Delete_RemovesBalancesFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            fixture.AddData(data);
            fixture.AddData(balanceBuilder.WithAccountId(id).Generate());

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            var balances = fixture.GetData<BalanceEntity>();
            Assert.IsEmpty(balances.Where(x => x.AccountId == id));
        }

        [Test]
        public async Task Delete_RemovesTransactionsFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            fixture.AddData(data);

            var balance = balanceBuilder.WithAccountId(id).Generate();
            fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            fixture.AddData(transaction);

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            var transactions = fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == id));
        }
    }
}
