namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task Put_ExistingCategory_ReturnUpdatedCategory()
        {
            var id = Guid.NewGuid();
            fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = builder.WithId(id).Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            CategoryModelAssert.Equal(body, result!.Data);
        }

        [Test]
        public async Task Put_ExistingCategory_UpdatesCategory()
        {
            var id = Guid.NewGuid();
            fixture.AddData(dataBuilder.WithId(id).Generate());

            var body = builder.WithId(id).Generate();
            await client.PutAsync($"{baseEndpoint}/{id}", body);

            await AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingCategory_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = builder.WithId(id).Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Category with id {id}", result!.Message);
        }
    }
}
