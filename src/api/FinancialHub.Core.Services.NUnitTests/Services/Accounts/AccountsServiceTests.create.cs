namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        [TestCase(Description = "Create valid account", Category = "Create")]
        public async Task CreateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.accountModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountModel>(It.IsAny<AccountEntity>()))
                .Returns<AccountEntity>((ent) => this.mapper.Map<AccountModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<AccountEntity>(It.IsAny<AccountModel>()))
                .Returns<AccountModel>((model) => this.mapper.Map<AccountEntity>(model))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);

            this.mapperWrapper.Verify(x => x.Map<AccountModel>(It.IsAny<AccountEntity>()),Times.Once);
            this.repository.Verify(x => x.CreateAsync(It.IsAny<AccountEntity>()), Times.Once);
            this.mapperWrapper.Verify(x => x.Map<AccountEntity>(It.IsAny<AccountModel>()),Times.Once);
        }
    }
}
