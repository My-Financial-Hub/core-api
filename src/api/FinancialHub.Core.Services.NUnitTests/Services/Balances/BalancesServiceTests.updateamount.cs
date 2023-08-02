namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task UpdateAmountAsync_UpdatesBalanceAmount()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var entity = this.mapper.Map<BalanceEntity>(model);
            var amount = this.random.Next(1000, 10000);
            this.repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(entity);

            this.repository
                .Setup(x => x.ChangeAmountAsync(id, amount))
                .ReturnsAsync(entity)
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.UpdateAmountAsync(id, amount);

            this.repository.Verify(x => x.ChangeAmountAsync(id, amount), Times.Once);
        }

        [Test]
        public async Task UpdateAmountAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var entity = this.mapper.Map<BalanceEntity>(model);
            var amount = this.random.Next(1000, 10000);

            this.repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(entity);

            this.repository
                .Setup(x => x.ChangeAmountAsync(id, amount))
                .ReturnsAsync(entity);

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account));

            this.SetUpMapper();

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAmountAsync_NonExistingBalanceId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var model = this.balanceModelBuilder.WithId(id).Generate();
            var entity = this.mapper.Map<BalanceEntity>(model);
            var amount = this.random.Next(1000, 10000);

            this.repository
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(BalanceEntity))
                .Verifiable();
            this.repository
                .Setup(x => x.ChangeAmountAsync(id, amount))
                .Verifiable();

            var result = await this.service.UpdateAmountAsync(id, amount);

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Balance with id {id}", result.Error.Message);

            this.repository.Verify(x => x.GetByIdAsync(model.Id.GetValueOrDefault()), Times.Once);
            this.repository.Verify(x => x.UpdateAsync(It.IsAny<BalanceEntity>()), Times.Never);
        }
    }
}
