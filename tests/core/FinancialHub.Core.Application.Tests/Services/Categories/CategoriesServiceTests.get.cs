using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        public async Task GetByUsersAsync_ValidUser_ReturnsCategories()
        {
            var categoriesMock = this.categoryModelBuilder.Generate(random.Next(10, 100));
            
            this.provider
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(categoriesMock.ToArray())
                .Verifiable();

            var result = await this.service.GetAllAsync();

            Assert.IsInstanceOf<ServiceResult<ICollection<CategoryDto>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(categoriesMock.Count, result.Data!.Count);

            this.provider.Verify(x => x.GetAllAsync(),Times.Once());
        }
    }
}
