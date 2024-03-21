using FinancialHub.Core.Domain.DTOS.Balances;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;
using static FinancialHub.Common.Results.Errors.ValidationError;

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
        public async Task CreateAsync_ValidBalance_CreatesBalance()
        {
            var createBalance = this.createBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(createBalance.AccountId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.CreateAsync(createBalance);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidBalance_ReturnsBalance()
        {
            var createBalance = this.createBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(ServiceResult.Success);
            this.accountValidator
                .Setup(x => x.ExistsAsync(createBalance.AccountId))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<BalanceModel>()))
                .Returns<BalanceModel>(async (x) => await Task.FromResult(x));

            var result = await this.service.CreateAsync(createBalance);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<BalanceDto>>(result);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_ReturnsValidationError()
        {
            var expectedMessage = "Validation error";
            var createBalance = this.createBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<FieldValidationError>()));

            var result = await this.service.CreateAsync(createBalance);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidBalance_DoNotCreatesBalance()
        {
            var createBalance = this.createBalanceDtoBuilder.Generate();
            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(new ValidationError("Validation error", Array.Empty<FieldValidationError>()));
            
            await this.service.CreateAsync(createBalance);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsNotFoundError()
        {
            var createBalance = this.createBalanceDtoBuilder.Generate();
            var expectedErrorMessage = $"Not found Account with id {createBalance.AccountId}";

            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(new NotFoundError($"Not found Account with id {createBalance.AccountId}"));

            var result = await this.service.CreateAsync(createBalance);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_DoNotCreatesBalance()
        {
            var createBalance = this.createBalanceDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(createBalance))
                .ReturnsAsync(new NotFoundError($"Not found Account with id {createBalance.AccountId}"));

            await this.service.CreateAsync(createBalance);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<BalanceModel>()), Times.Never);
        }
    }
}
