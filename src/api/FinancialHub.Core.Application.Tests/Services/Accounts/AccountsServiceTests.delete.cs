namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        public async Task DeleteAsync_RepositorySuccess_ReturnsAccountModel()
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
