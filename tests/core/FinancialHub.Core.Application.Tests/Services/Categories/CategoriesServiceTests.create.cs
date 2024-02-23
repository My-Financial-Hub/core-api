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
        public async Task CreateAsync_ValidCategoryModel_ReturnsCategoryModel()
        {
            var model = this.categoryModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<CategoryModel>()))
                .Returns<CategoryModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var createCategory = createCategoryDtoBuilder.Generate();
            var result = await this.service.CreateAsync(createCategory);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<CategoryDto>>(result);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<CategoryModel>()), Times.Once);
        }
    }
}
