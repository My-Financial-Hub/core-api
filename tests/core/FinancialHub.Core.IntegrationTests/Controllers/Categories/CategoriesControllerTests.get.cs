using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.IntegrationTests.Controllers.Categories
{
    public partial class CategoriesControllerTests
    {
        [Test]
        public async Task GetAll_ReturnCategories()
        {
            var data = categoryBuilder.Generate(10);
            fixture.AddData(data.ToArray());

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<CategoryDto>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }
    }
}
