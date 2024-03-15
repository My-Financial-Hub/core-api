using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        private CreateCategoryDtoBuilder createCategoryDtoBuilder;
        protected void AddCreateCategoryBuilder()
        {
            createCategoryDtoBuilder = new CreateCategoryDtoBuilder();
        }

        [Test]
        public async Task CreateAsync_ValidCategory_ReturnsCategoryModel()
        {
            var createCategory = createCategoryDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createCategory))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<CategoryModel>()))
                .Returns<CategoryModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(createCategory);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<CategoryDto>>(result);
        }

        [Test]
        public async Task CreateAsync_ValidCategory_CreatesCategory()
        {
            var createCategory = createCategoryDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createCategory))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<CategoryModel>()))
                .Returns<CategoryModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.CreateAsync(createCategory);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_ReturnsValidationError()
        {
            var createCategory = createCategoryDtoBuilder.Generate();
            var expectedMessage = "Category Validation Error";

            this.validator
                .Setup(x => x.ValidateAsync(createCategory))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<ValidationError.FieldValidationError>()));

            var result = await this.service.CreateAsync(createCategory);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidCategory_DoNotCreatesCategory()
        {
            var createCategory = createCategoryDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createCategory))
                .ReturnsAsync(new ValidationError("Category Validation Error", Array.Empty<ValidationError.FieldValidationError>()));

            await this.service.CreateAsync(createCategory);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<CategoryModel>()), Times.Never);
        }
    }
}
