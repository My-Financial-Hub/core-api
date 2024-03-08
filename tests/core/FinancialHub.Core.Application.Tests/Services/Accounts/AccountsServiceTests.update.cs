using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Accounts;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        private UpdateAccountDtoBuilder updateAccountDtoBuilder;
        protected void AddUpdateAccountBuilder()
        {
            updateAccountDtoBuilder = new UpdateAccountDtoBuilder();
        }

        [Test]
        public async Task UpdateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var updateAccountDto = this.updateAccountDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateAccountDto))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<AccountModel>()))
                .Returns<Guid, AccountModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.UpdateAsync(id, updateAccountDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);
        }

        [Test]
        public async Task UpdateAsync_InvalidAccountModel_ReturnsValidationError()
        {
            var expectedMessage = "Account validation error";
            var model = accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var updateAccountDto = this.updateAccountDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateAccountDto))
                .ReturnsAsync(new ValidationError(expectedMessage, Array.Empty<ValidationError.FieldValidationError>()));

            var result = await this.service.UpdateAsync(id, updateAccountDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);
        }

        [Test]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsNotFoundError()
        {
            var expectedMessage = "Account Not found error";

            var model = accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            var updateAccountDto = this.updateAccountDtoBuilder.Generate();

            this.validator
                .Setup(x => x.ValidateAsync(updateAccountDto))
                .ReturnsAsync(ServiceResult.Success);
            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedMessage));

            var result = await this.service.UpdateAsync(id, updateAccountDto);

            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedMessage, result.Error!.Message);
        }
    }
}
