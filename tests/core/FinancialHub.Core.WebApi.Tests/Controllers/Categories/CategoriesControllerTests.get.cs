using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        [TestCase(Description = "Get returns Ok", Category = "Create")]
        public async Task GetMyCategories_ServiceSuccess_ReturnsOk()
        {
            var mockResult = new ServiceResult<ICollection<CategoryDto>>(
                categoryDtoBuilder.Generate(random.Next(0, 10))
            );

            this.mockService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.GetCategories();
            var result = (ObjectResult)response;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<ListResponse<CategoryDto>>(result.Value);

            var listResponse = result.Value as ListResponse<CategoryDto>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}
