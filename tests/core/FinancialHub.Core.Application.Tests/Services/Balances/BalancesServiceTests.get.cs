using FinancialHub.Core.Domain.Tests.Assertions.Models;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        [Test]
        public async Task GetByAccountAsync_ValidAccount_ReturnsBalances()
        {
            var firstModel = this.balanceModelBuilder.Generate();
            var balances = this.balanceModelBuilder
                .WithAccount(firstModel.Account)
                .Generate(random.Next(5, 10));
            var acountId = firstModel.Account.Id.GetValueOrDefault();

            this.accountsProvider
                .Setup(x => x.GetByIdAsync(acountId))
                .ReturnsAsync(firstModel.Account)
                .Verifiable();
            this.provider
                .Setup(x => x.GetAllByAccountAsync(acountId))
                .ReturnsAsync(balances.ToArray())
                .Verifiable();

            var result = await this.service.GetAllByAccountAsync(acountId);

            Assert.IsInstanceOf<ServiceResult<ICollection<BalanceModel>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(balances.Count, result.Data!.Count);

            this.provider.Verify(x => x.GetAllByAccountAsync(acountId), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsBalance()
        {
            var entity = this.balanceModelBuilder.Generate();

            this.provider
                .Setup(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()))
                .ReturnsAsync(entity)
                .Verifiable();

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsFalse(result.HasError);
            BalanceModelAssert.Equal(entity, result.Data!);

            this.provider.Verify(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNotFoundError()
        {
            var entity = this.balanceModelBuilder.Generate();

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<BalanceModel>>(result);
            Assert.IsTrue(result.HasError);
        }
    }
}
