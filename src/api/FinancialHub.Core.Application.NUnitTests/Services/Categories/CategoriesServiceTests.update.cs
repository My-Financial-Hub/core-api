namespace FinancialHub.Core.Application.NUnitTests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        [TestCase(Description = "Update valid Category", Category = "Update")]
        public async Task UpdateAsync_ValidCategoryModel_ReturnsCategoryModel()
        {
            var model = this.categoryModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<CategoryEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<CategoryEntity>()))
                .Returns<CategoryEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<CategoryModel>(It.IsAny<CategoryEntity>()))
                .Returns<CategoryEntity>((ent) => this.mapper.Map<CategoryModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<CategoryEntity>(It.IsAny<CategoryModel>()))
                .Returns<CategoryModel>((model) => this.mapper.Map<CategoryEntity>(model))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<CategoryEntity>()), Times.Once);

            this.mapperWrapper.Verify(x => x.Map<CategoryModel>(It.IsAny<CategoryEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<CategoryEntity>(It.IsAny<CategoryModel>()), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update non existing Category", Category = "Update")]
        public async Task UpdateAsync_NonExistingCategoryId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var model = this.categoryModelBuilder
                .WithId(id)
                .Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(CategoryEntity))
                .Verifiable();

            this.repository
                .Setup(x => x.UpdateAsync(It.IsAny<CategoryEntity>()))
                .Returns<CategoryEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);
            Assert.IsTrue(result.HasError);

            this.repository.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<CategoryEntity>()), Times.Never);
        }
    }
}
