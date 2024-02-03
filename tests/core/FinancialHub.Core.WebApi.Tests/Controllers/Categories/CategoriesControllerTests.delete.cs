namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        [TestCase(Description = "Delete Returns NoContent", Category = "Delete")]
        public async Task DeleteMyCategories_ServiceSuccess_ReturnsNoContent()
        {
            var response = await this.controller.DeleteCategory(Guid.NewGuid());

            var result = response as ObjectResult;

            Assert.IsNull(result?.Value);
        }
    }
}
