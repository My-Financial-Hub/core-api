using System.Linq;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        //TODO: change mock when filter by user
        [Test]
        [TestCase(Description = "Get by user sucess return",Category = "Get")]
        public async Task GetByUsersAsync_ValidUser_ReturnsCategories()
        {
            var entitiesMock = this.CreateCategories();
            
            this.repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(entitiesMock.ToArray())
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<CategoryModel>>(It.IsAny<IEnumerable<CategoryEntity>>()))
                .Returns<IEnumerable<CategoryEntity>>((ent) => this.mapper.Map<IEnumerable<CategoryModel>>(ent))
                .Verifiable();

            var result = await this.service.GetAllByUserAsync(string.Empty);

            Assert.IsInstanceOf<ServiceResult<ICollection<CategoryModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count(), result.Data!.Count);

            this.mapperWrapper.Verify(x => x.Map<IEnumerable<CategoryModel>>(It.IsAny<IEnumerable<CategoryEntity>>()),Times.Once);
            this.repository.Verify(x => x.GetAllAsync(),Times.Once());
        }
    }
}
