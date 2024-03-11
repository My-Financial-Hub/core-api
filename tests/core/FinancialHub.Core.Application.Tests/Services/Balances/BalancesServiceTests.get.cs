using FinancialHub.Core.Domain.DTOS.Balances;

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

            this.accountValidator
                .Setup(x => x.ExistsAsync(acountId))
                .ReturnsAsync(ServiceResult.Success)
                .Verifiable();
            this.provider
                .Setup(x => x.GetAllByAccountAsync(acountId))
                .ReturnsAsync(balances.ToArray())
                .Verifiable();

            var result = await this.service.GetAllByAccountAsync(acountId);

            Assert.IsInstanceOf<ServiceResult<ICollection<BalanceDto>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(balances.Count, result.Data!.Count);

            this.provider.Verify(x => x.GetAllByAccountAsync(acountId), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsBalance()
        {
            var entity = this.balanceModelBuilder.Generate();

            this.validator
                .Setup(x => x.ExistsAsync(entity.Id.GetValueOrDefault()))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()))
                .ReturnsAsync(entity)
                .Verifiable();

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Data);

            this.provider.Verify(x => x.GetByIdAsync(entity.Id.GetValueOrDefault()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNotFoundError()
        {
            var entity = this.balanceModelBuilder.Generate();
            var expectedErrorMessage = $"Balance not found with id {entity.Id}";

            this.validator
                .Setup(x => x.ExistsAsync(entity.Id.GetValueOrDefault()))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.GetByIdAsync(entity.Id.GetValueOrDefault());

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }
    }
}
