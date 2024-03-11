using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;
using static FinancialHub.Common.Results.Errors.ValidationError;

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
        public async Task UpdateAsync_ValidBalance_UpdatesBalance()
        {
            var id = Guid.NewGuid();
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(updateBalance.AccountId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.UpdateAsync(id, updateBalance);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidBalance_ReturnsBalance()
        {
            var id = Guid.NewGuid();
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(updateBalance.AccountId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()))
                .Returns<Guid, BalanceModel>(async (_, x) => await Task.FromResult(x));

            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task UpdateAsync_NonExistingBalanceId_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var expectedErrorMessage = $"Not found Balance with id {id}";
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_NonExistingBalanceId_DoNotUpdateBalance()
        {
            var id = Guid.NewGuid();
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotFoundError($"Not found Balance with id {id}"));

            await this.service.UpdateAsync(id, updateBalance);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var expectedErrorMessage = $"Not found Balance with id {id}";
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(new ValidationError(expectedErrorMessage, Array.Empty<FieldValidationError>()));

            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidBalance_DoNotUpdateBalance()
        {
            var id = Guid.NewGuid();
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(new ValidationError("Validation error", Array.Empty<FieldValidationError>()));

            await this.service.UpdateAsync(id, updateBalance);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Never);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var id = Guid.NewGuid();
            var expectedErrorMessage = $"Account not found with id {id}";
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(updateBalance.AccountId))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.UpdateAsync(id, updateBalance);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_DoNotUpdateBalance()
        {
            var id = Guid.NewGuid();
            var updateBalance = this.updateBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(updateBalance.AccountId))
                .ReturnsAsync(new NotFoundError($"Account not found with id {id}"));

            await this.service.UpdateAsync(id, updateBalance);

            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<BalanceModel>()), Times.Never);
        }
    }
}
