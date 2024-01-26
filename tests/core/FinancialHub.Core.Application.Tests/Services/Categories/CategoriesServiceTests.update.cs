using Moq;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidCategoryModel_ReturnsCategoryModel()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            var updatedModel = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, updatedModel))
                .ReturnsAsync(updatedModel)
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<CategoryModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_NonExistingCategoryId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(CategoryModel))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);
            Assert.IsTrue(result.HasError);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<CategoryModel>()), Times.Never);
        }
    }
}
