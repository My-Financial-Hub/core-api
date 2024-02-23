namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = categoryBuilder.WithId(id).Generate();
            fixture.AddData(data);

            var response = await client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesCategoryFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = categoryBuilder.WithId(id).Generate();
            fixture.AddData(data);

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            var categories = fixture.GetData<CategoryEntity>();
            Assert.IsEmpty(categories);
        }

        [Test]
        public async Task Delete_RemovesTransactionsWithCategoryFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = categoryBuilder.WithId(id).Generate();
            fixture.AddData(data);

            var transactionsData = transactionBuilder
                .WithCategoryId(data.Id)
                .Generate();
            fixture.AddData(transactionsData);

            await client.DeleteAsync($"{baseEndpoint}/{id}");

            var categories = fixture.GetData<CategoryEntity>();
            var transactions = fixture.GetData<TransactionEntity>();

            Assert.IsEmpty(categories);
            Assert.IsEmpty(transactions);
        }
    }
}
