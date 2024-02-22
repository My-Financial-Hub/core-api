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
        [TestCase(Description = "Create valid account", Category = "Create")]
        public async Task CreateAsync_ValidAccountModel_ReturnsAccountModel()
        {
            var model = this.createAccountDtoBuilder.Generate();

            this.provider
                .Setup(x => x.CreateAsync(It.IsAny<AccountModel>()))
                .Returns<AccountModel>(async (x) => await Task.FromResult(x))
                .Verifiable();

            var result = await this.service.CreateAsync(model);

            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);

            this.provider.Verify(x => x.CreateAsync(It.IsAny<AccountModel>()), Times.Once);
        }
    }
}
