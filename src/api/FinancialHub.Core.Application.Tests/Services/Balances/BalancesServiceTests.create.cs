namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidatesIfAccountExists()
        {
            var model = this.balanceModelBuilder.Generate();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            this.accountsRepository.Verify(x => x.GetByIdAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CreatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.GetByIdAsync(model.Id.GetValueOrDefault()))
                .ReturnsAsync(this.mapper.Map<BalanceEntity>(model))
                .Verifiable();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidBalanceModel_ReturnsBalanceModel()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.accountsRepository
                .Setup(x => x.GetByIdAsync(model.AccountId))
                .ReturnsAsync(this.mapper.Map<AccountEntity>(model.Account))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsNotFoundError()
        {
            var model = this.balanceModelBuilder.Generate();

            this.repository
                .Setup(x => x.CreateAsync(It.IsAny<BalanceEntity>()))
                .Returns<BalanceEntity>(async (x) => await Task.FromResult(x))
                .Verifiable();

            this.SetUpMapper();

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual($"Not found Account with id {model.AccountId}", result.Error!.Message);

            this.repository.Verify(x => x.CreateAsync(It.IsAny<BalanceEntity>()), Times.Never);
        }
    }
}
