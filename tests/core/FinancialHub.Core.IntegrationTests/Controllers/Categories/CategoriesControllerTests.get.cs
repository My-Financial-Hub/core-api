namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task GetAll_ReturnCategories()
        {
            var data = dataBuilder.Generate(10);
            fixture.AddData(data.ToArray());

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<CategoryModel>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }
    }
}
