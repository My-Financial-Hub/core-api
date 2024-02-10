namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        [Test]
        public async Task GetAll_ReturnActiveTransactions()
        {
            var data = InsertTransactions();

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(data.Length, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_DoesNotReturnNotActiveTransactions()
        {
            InsertTransactions(false);

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.Zero(result!.Data.Count);
        }
    }
}
