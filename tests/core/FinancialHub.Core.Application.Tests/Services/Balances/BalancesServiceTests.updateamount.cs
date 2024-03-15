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

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
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

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model);
            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount))
                .ReturnsAsync(model); 

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAmountAsync_NonExistingBalanceId_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var amount = this.random.Next(1000, 10000);
            var expectedErrorMessage = $"Not found Balance with id {id}";

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));
            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount));

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAmountAsync_NonExistingBalanceId_DoNotUpdateAmount()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var amount = this.random.Next(1000, 10000);
            var expectedErrorMessage = $"Not found Balance with id {id}";

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));
            this.provider
                .Setup(x => x.UpdateAmountAsync(id, amount));

            await this.service.UpdateAmountAsync(id, amount);

            this.provider.Verify(x => x.UpdateAmountAsync(id, amount), Times.Never);
        }
    }
}
