using System.Linq;

namespace FinancialHub.Core.Services.NUnitTests.Services
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
            Assert.AreEqual(entitiesMock.Count(), result.Data.Count);

            this.mapperWrapper.Verify(x => x.Map<IEnumerable<CategoryModel>>(It.IsAny<IEnumerable<CategoryEntity>>()),Times.Once);
            this.repository.Verify(x => x.GetAllAsync(),Times.Once());
        }


        [Test]
        [TestCase(Description = "Get by user repository exception", Category = "Get")]
        public void GetByUsersAsync_RepositoryException_ThrowsException()
        {
            var entitiesMock = this.CreateCategories();

            var exc = new Exception("mock");
            this.repository
                .Setup(x => x.GetAllAsync())
                .Throws(exc)
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<IEnumerable<CategoryModel>>(It.IsAny<IEnumerable<CategoryEntity>>()))
                .Returns<IEnumerable<CategoryEntity>>((ent) => this.mapper.Map<IEnumerable<CategoryModel>>(ent))
                .Verifiable();

            var exception = Assert.ThrowsAsync<Exception>(
                async ()=> await this.service.GetAllByUserAsync(string.Empty)
            );

            Assert.IsInstanceOf(exc.GetType(), exception);
            this.repository.Verify(x => x.GetAllAsync(), Times.Once());
        }
    }
}
