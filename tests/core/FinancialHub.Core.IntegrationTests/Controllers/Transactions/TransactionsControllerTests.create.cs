namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        [Test]
        public async Task Post_ValidTransaction_ReturnCreatedTransaction()
        {
            var data = CreateValidTransaction();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            TransactionModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidTransaction_CreateTransaction()
        {
            var body = CreateValidTransaction();

            await client.PostAsync(baseEndpoint, body);

            await AssertGetExists(body);
        }
    }
}
