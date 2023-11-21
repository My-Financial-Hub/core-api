namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task UpdateAsync_ValidatesIfAccountExists()
        {
            var model = this.balanceModelBuilder.Generate();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account)
                .Verifiable();
            this.provider
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(model)
                .Verifiable();

            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            this.accountsProvider.Verify(x => x.GetByIdAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidatesIfBalanceExists()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceModel))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {id}", result.Error!.Message);
            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account)
                .Verifiable();

            await this.service.UpdateAsync(id, model);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account)
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAsync_NonExistingBalanceId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceModel))
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {model.Id}", result.Error!.Message);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), model);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error!.Message);
        }
    }
}
