namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        [TestCase(Description = "Create valid account", Category = "Create")]
        public async Task CreateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.accountModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<AccountModel>()))
                .Returns<AccountModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<AccountModel>()), Times.Once);
        }
    }
}
