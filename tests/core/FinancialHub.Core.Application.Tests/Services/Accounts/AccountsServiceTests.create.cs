using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        private CreateAccountDtoBuilder createAccountDtoBuilder;
        protected void AddCreateAccountBuilder()
        {
            createAccountDtoBuilder = new CreateAccountDtoBuilder();
        }

        [Test]
        public async Task CreateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.createAccountDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<AccountModel>()))
                .Returns<AccountModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<AccountModel>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_ReturnsValidationError()
        {
            var model = this.createAccountDtoBuilder.Generate();
            var expectedMessage = "Account validation error";

            this.validator
                .Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<ValidationError.FieldValidationError>()));

            var result = await this.service.CreateAsync(model);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<ValidationError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }

        [Test]
        public async Task CreateAsync_InvalidAccountModel_DoNotCreateAccount()
        {
            var model = this.createAccountDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(new ValidationError("Account validation error", Array.Empty<ValidationError.FieldValidationError>()));

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<AccountModel>()))
                .Returns<AccountModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            await this.service.CreateAsync(model);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<AccountModel>()), Times.Never);
        }
    }
}
