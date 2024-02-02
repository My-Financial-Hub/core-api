using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        [TestCase(Description = "Create valid category returns Ok", Category = "Create")]
        public async Task CreateCategory_Valid_ReturnsOk()
        {
            var body = this.createCategoryDtoBuilder.Generate();
            var resultMock = this.categoryDtoBuilder
                .FromCreateDto(body)
                .Generate();
            var mockResult = new ServiceResult<CategoryDto>(resultMock);

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateCategory(body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<CategoryDto>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<CategoryDto>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Create invalid Category returns BadRequest", Category = "Create")]
        public async Task CreateCategory_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.createCategoryDtoBuilder.Generate();
            var resultMock = this.categoryDtoBuilder
                .FromCreateDto(body)
                .Generate();
            var mockResult = new ServiceResult<CategoryDto>(resultMock, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.CreateAsync(body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.CreateCategory(body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error!.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error!.Message, listResponse?.Message);

            this.mockService.Verify(x => x.CreateAsync(body), Times.Once);
        }
    }
}
