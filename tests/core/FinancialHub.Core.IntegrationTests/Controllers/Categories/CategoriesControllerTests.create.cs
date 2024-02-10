namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task Post_ValidCategory_ReturnCreatedCategory()
        {
            var data = builder.Generate();

            var response = await client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<CategoryModel>>();
            Assert.IsNotNull(result?.Data);
            CategoryModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidCategory_CreateCategory()
        {
            var body = builder.Generate();

            await client.PostAsync(baseEndpoint, body);

            await AssertGetExists(body);
        }
    }
}
