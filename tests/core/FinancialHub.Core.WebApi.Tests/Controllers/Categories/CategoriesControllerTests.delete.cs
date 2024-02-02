namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        [TestCase(Description = "Delete Returns NoContent", Category = "Delete")]
        public async Task DeleteMyCategories_ServiceSuccess_ReturnsNoContent()
        {
            var mock = this.categoryDtoBuilder.Generate();
            var response = await this.controller.DeleteCategory(mock.Id.GetValueOrDefault());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
