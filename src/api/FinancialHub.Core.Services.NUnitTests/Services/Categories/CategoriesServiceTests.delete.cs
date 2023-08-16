namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class CategoriesServiceTests
    {
        [Test]
        [TestCase(Description = "Update valid Category", Category = "Delete")]
        public async Task DeleteAsync_RepositorySuccess_ReturnsCategoryModel()
        {
            var expectedResult = random.Next(1,100);
            var guid = Guid.NewGuid();
            this.repository
                .Setup(x => x.DeleteAsync(guid))
                .Returns(async () => await Task.FromResult(expectedResult))
                .Verifiable();

            var result = await this.service.DeleteAsync(guid);

            Assert.IsInstanceOf<ServiceResult<int>>(result);
            Assert.AreEqual(expectedResult,result.Data);
            this.repository.Verify(x => x.DeleteAsync(guid), Times.Once);
        }
    }
}
