using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        private UpdateBalanceDtoBuilder updateBalanceDtoBuilder;
        protected void AddUpdateBalanceBuilder()
        {
            updateBalanceDtoBuilder = new UpdateBalanceDtoBuilder();
        }
        
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

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), updateBalance);

            this.accountsProvider.Verify(x => x.GetByIdAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidatesIfBalanceExists()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var expectedErrorMessage = $"Not found Balance with id {id}";
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceModel))
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
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

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            await this.service.UpdateAsync(id, updateBalance);

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

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAsync_NonExistingBalanceId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var expectedErrorMessage = $"Not found Balance with id {model.Id}";
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceModel))
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var model = this.balanceModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var expectedErrorMessage = $"Not found Account with id {model.AccountId}";

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.UpdateAsync(model.Id.GetValueOrDefault(), updateBalance);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }
    }
}
