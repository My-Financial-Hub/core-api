namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            var id = Guid.NewGuid();

            var data = balanceBuilder.WithId(id).Generate();
            fixture.AddData(data);

            var response = await client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesBalanceFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = balanceBuilder.WithId(id).Generate();
            fixture.AddData(data);

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            var result = fixture.GetData<BalanceEntity>();
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Delete_RemovesTransactionsFromDatabase()
        {
            var account = accountBuilder.WithId(Guid.NewGuid()).Generate();
            fixture.AddData(account);

            var balance = balanceBuilder.WithAccountId(account.Id).Generate();
            fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            fixture.AddData(transaction);

            await client.DeleteAsync($"{baseEndpoint}/{balance.Id}");

            var transactions = fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == account.Id));
        }

        [Test]
        public async Task Delete_RemovesTransactionsOnlyFromTheSelectedBalanceFromDatabase()
        {
            var account = accountBuilder.WithId(Guid.NewGuid()).Generate();
            fixture.AddData(account);

            var balance = balanceBuilder.WithAccountId(account.Id).Generate();
            fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            fixture.AddData(transaction);

            var balance2 = balanceBuilder.WithAccountId(account.Id).Generate();
            fixture.AddData(balance2);

            var transaction2 = transactionBuilder
                .WithBalanceId(balance2.Id)
                .WithCategoryId(transaction.CategoryId)
                .Generate();
            fixture.AddData(transaction2);

            await client.DeleteAsync($"{baseEndpoint}/{balance.Id}");

            var transactions = fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == account.Id));
            Assert.IsNotEmpty(transactions.Where(x => x.BalanceId == balance2.Id && x.Balance.AccountId == account.Id));
        }
    }
}
