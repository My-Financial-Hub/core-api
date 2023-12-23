namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidatesIfAccountExists()
        {
            var model = this.balanceModelBuilder.Generate();

            await this.service.CreateAsync(model);

            this.accountsProvider.Verify(x => x.GetByIdAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CreatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account)
                .Verifiable();

            await this.service.CreateAsync(model);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var model = this.balanceModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account)
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsNotFoundError()
        {
            var model = this.balanceModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error!.Message);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Never);
        }
    }
}
