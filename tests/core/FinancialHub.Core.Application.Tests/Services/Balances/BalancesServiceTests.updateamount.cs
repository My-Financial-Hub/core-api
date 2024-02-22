using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task UpdateAmountAsync_UpdatesBalanceAmount()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var amount = this.random.Next(1000, 10000);
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model);

            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount))
                .ReturnsAsync(model)
                .Verifiable();

            await this.service.UpdateAmountAsync(id, amount);

            this.provider.Verify(x => x.UpdateAmountAsync(id, amount), Times.Once);
        }

        [Test]
        public async Task UpdateAmountAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var amount = this.random.Next(1000, 10000);

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model);

            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount))
                .ReturnsAsync(model);

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(model.Account);

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAmountAsync_NonExistingBalanceId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var amount = this.random.Next(1000, 10000);
            var expectedErrorMessage = $"Not found Balance with id {id}";

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceModel))
                .Verifiable();
            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount))
                .Verifiable();
            this.errorMessageProvider
                .Setup(x => x.NotFoundMessage(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(expectedErrorMessage);

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);

            this.provider.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.provider.Verify(x => x.UpdateAmountAsync(id, amount), Times.Never);
        }
    }
}
