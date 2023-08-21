namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        [TestCase(Description = "Create valid Category", Category = "Create")]
        public async Task CreateAsync_ValidCategoryModel_ReturnsCategoryModel()
        {
            var model = this.categoryModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<CategoryEntity>()))
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

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);

            this.mapperWrapper.Verify(x => x.Map<CategoryModel>(It.IsAny<CategoryEntity>()),Times.Once);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<CategoryEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<CategoryEntity>(It.IsAny<CategoryModel>()),Times.Once);
        }
    }
}
