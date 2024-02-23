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
        [TestCase(Description = "Update valid account", Category = "Update")]
        public async Task UpdateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(model)
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<AccountModel>()))
                .Returns<Guid, AccountModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var updateAccountDto = this.updateAccountDtoBuilder.Generate();
            var result = await this.service.UpdateAsync(id, updateAccountDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<AccountModel>()), Times.Once);
        }

        [Test]
        [TestCase(Description = "Update non existing account", Category = "Update")]
        public async Task UpdateAsync_NonExistingAccountId_ReturnsResultError()
        {
            var model = accountModelBuilder.Generate();
            var id = model.Id.GetValueOrDefault();

            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(default(AccountModel))
                .Verifiable();

            this.provider
                .Setup(x => x.UpdateAsync(id, It.IsAny<AccountModel>()))
                .Returns<Guid ,AccountModel>(async (_, x) => await Task.FromResult(x))
                .Verifiable();

            var updateAccountDto = this.updateAccountDtoBuilder.Generate();
            var result = await this.service.UpdateAsync(id, updateAccountDto);

            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);
            Assert.IsTrue(result.HasError);

            this.provider.Verify(x => x.GetByIdAsync(id), Times.Once);
            this.provider.Verify(x => x.UpdateAsync(id, It.IsAny<AccountModel>()), Times.Never);
        }
    }
}
