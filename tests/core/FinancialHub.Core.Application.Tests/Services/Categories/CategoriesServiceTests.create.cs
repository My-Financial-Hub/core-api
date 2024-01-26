namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidCategoryModel_ReturnsCategoryModel()
        {
            var model = this.categoryModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<CategoryModel>()))
                .Returns<CategoryModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<CategoryModel>>(result);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);
        }
    }
}
