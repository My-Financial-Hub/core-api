namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_ExistingTransaction_ReturnUpdatedTransaction()
        {
            var data = InsertTransaction(true);

            var body = modelBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .WithId(data.Id.GetValueOrDefault())
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{data.Id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            TransactionModelAssert.Equal(body, result!.Data);
        }

        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_ExistingTransaction_UpdatesTransaction()
        {
            var data = InsertTransaction(true);

            var body = modelBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .WithId(data.Id.GetValueOrDefault())
                .Generate();
            await client.PutAsync($"{baseEndpoint}/{data.Id}", body);

            await AssertGetExists(body);
        }

        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_NonExistingTransaction_ReturnNotFoundError()
        {
            var body = CreateValidTransaction();

            var response = await client.PutAsync($"{baseEndpoint}/{body.Id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Transaction with id {body.Id}", result!.Message);
        }
    }
}
