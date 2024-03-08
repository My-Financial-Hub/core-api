using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        private CreateBalanceDtoBuilder createBalanceDtoBuilder;
        protected void AddCreateBalanceBuilder()
        {
            createBalanceDtoBuilder = new CreateBalanceDtoBuilder();
        }

        [Test]
        public async Task CreateAsync_ValidatesIfAccountExists()
        {
            var model = this.createBalanceDtoBuilder.Generate();

            await this.service.CreateAsync(model);

            this.accountValidator.Verify(x => x.ExistsAsync(model.AccountId), Times.Once);
        }

        [Test]
        public async Task CreateAsync_CreatesBalance()
        {
            var model = this.balanceModelBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var createBalance = this.createBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            await this.service.CreateAsync(createBalance);

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

            var createBalance = this.createBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.CreateAsync(createBalance);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsNotFoundError()
        {
            var model = this.balanceModelBuilder.Generate();
            var expectedErrorMessage = $"Not found Account with id {model.AccountId}";

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var createBalance = this.createBalanceDtoBuilder
                .WithAccountId(model.AccountId)
                .Generate();
            var result = await this.service.CreateAsync(createBalance);

            Assert.IsTrue(result.HasError);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Never);
        }
    }
}
