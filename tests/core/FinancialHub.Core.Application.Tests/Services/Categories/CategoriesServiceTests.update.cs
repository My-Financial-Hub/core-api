using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;
using static FinancialHub.Common.Results.Errors.ValidationError;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        private UpdateCategoryDtoBuilder updateCategoryDtoBuilder;
        protected void AddUpdateCategoryBuilder()
        {
            updateCategoryDtoBuilder = new UpdateCategoryDtoBuilder();
        }

        [Test]
        public async Task UpdateAsync_ValidCategory_ReturnsCategory()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updatedModel = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updateCategory = updateCategoryDtoBuilder.Generate();
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ValidateAsync(updateCategory))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, updatedModel))
                .ReturnsAsync(updatedModel);

            var result = await this.service.UpdateAsync(id, updateCategory);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<CategoryDto>>(result);
        }

        [Test]
        public async Task UpdateAsync_ValidCategory_UpdatesCategory()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updatedModel = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updateCategory = updateCategoryDtoBuilder.Generate();
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ValidateAsync(updateCategory))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, updatedModel))
                .ReturnsAsync(updatedModel);

            await this.service.UpdateAsync(id, updateCategory);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<CategoryModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_InvalidCategory_ReturnsValidationError()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();
            var expectedMessage = "Validation error";

            var updatedModel = this.categoryModelBuilder
                .WithId(id)
                .Generate();
            var updateCategory = updateCategoryDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ValidateAsync(updateCategory))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<FieldValidationError>()));

            var result = await this.service.UpdateAsync(id, updateCategory);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidCategor_DoesNotCreateCategory()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updatedModel = this.categoryModelBuilder
                .WithId(id)
                .Generate();
            var updateCategory = updateCategoryDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ValidateAsync(updateCategory))
                .ReturnsAsync(new ValidationError("Validation error", Array.Empty<FieldValidationError>()));

            await this.service.UpdateAsync(id, updateCategory);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<CategoryModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_NonExistingCategoryId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var expectedMessage = $"Category with id {id} not found";
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedMessage))
                .Verifiable();

            var updateCategory = updateCategoryDtoBuilder.Generate();
            var result = await this.service.UpdateAsync(id, updateCategory);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_NonExistingCategoryId_DoesNotCreateCategory()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError("Category not found"))
                .Verifiable();

            var updateCategory = updateCategoryDtoBuilder.Generate();
            await this.service.UpdateAsync(id, updateCategory);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<CategoryModel>()), Times.Never);
        }
    }
}
