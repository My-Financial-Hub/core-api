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

            this.validator
                .Setup(x => x.ExistsAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.accountValidator
                .Setup(x => x.ExistsAsync(model.AccountId))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();

            var updateBalance = this.updateBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            await this.service.UpdateAsync(model.Id.GetValueOrDefault(), updateBalance);

            this.accountValidator.Verify(x => x.ExistsAsync(model.AccountId), Times.Once);
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
            this.validator
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

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

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.accountValidator
                .Setup(x => x.ExistsAsync(model.AccountId))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
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

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.accountValidator
                .Setup(x => x.ExistsAsync(model.AccountId))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
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
            this.validator
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

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

            this.validator
                .Setup(x => x.ExistsAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.accountValidator
                .Setup(x => x.ExistsAsync(model.AccountId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage))
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

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
