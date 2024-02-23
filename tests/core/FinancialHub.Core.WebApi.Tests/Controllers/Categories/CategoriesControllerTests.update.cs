using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class CategoriesControllerTests
    {
        [Test]
        [TestCase(Description = "Update valid Category returns Ok", Category = "Update")]
        public async Task UpdateCategory_Valid_ReturnsOk()
        {
            var body = this.updateCategoryDtoBuilder.Generate();
            var resultMock = this.categoryDtoBuilder
                .FromUpdateDto(body)
                .Generate();
            var guid = Guid.NewGuid();
            var mockResult = new ServiceResult<CategoryDto>(resultMock);

            this.mockService
                .Setup(x => x.UpdateAsync(guid, body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateCategory(guid, body);

            var result = response as ObjectResult;

            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsInstanceOf<SaveResponse<CategoryDto>>(result?.Value);

            var listResponse = result?.Value as SaveResponse<CategoryDto>;
            Assert.AreEqual(mockResult.Data, listResponse?.Data);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update invalid Category returns BadRequest", Category = "Update")]
        public async Task UpdateCategory_Invalid_ReturnsBadRequest()
        {
            var errorMessage = $"Invalid thing : {Guid.NewGuid()}";
            var body = this.updateCategoryDtoBuilder.Generate();
            var resultMock = this.categoryDtoBuilder
                .FromUpdateDto(body)
                .Generate();
            var guid = Guid.NewGuid();

            var mockResult = new ServiceResult<CategoryDto>(resultMock, new InvalidDataError(errorMessage));

            this.mockService
                .Setup(x => x.UpdateAsync(guid,body))
                .ReturnsAsync(mockResult)
                .Verifiable();

            var response = await this.controller.UpdateCategory(guid,body);

            var result = response as ObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
            Assert.IsInstanceOf<ValidationErrorResponse>(result?.Value);

            var listResponse = result?.Value as ValidationErrorResponse;
            Assert.AreEqual(mockResult.Error!.Code, listResponse?.Code);
            Assert.AreEqual(mockResult.Error!.Message, listResponse?.Message);

            this.mockService.Verify(x => x.UpdateAsync(guid, body), Times.Once);
        }
    }
}
