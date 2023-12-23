namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        [TestCase(Description = "Update valid account", Category = "Update")]
        public async Task UpdateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<AccountModel>()))
                .Returns<Guid, AccountModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<AccountModel>()), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update non existing account", Category = "Update")]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var model = this.accountModelBuilder
                .WithId(id)
                .Generate();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(AccountModel))
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<AccountModel>()))
                .Returns<Guid ,AccountModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsInstanceOf<ServiceResult<AccountModel>>(result);
            Assert.IsTrue(result.HasError);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<AccountModel>()), Times.Never);
        }
    }
}
