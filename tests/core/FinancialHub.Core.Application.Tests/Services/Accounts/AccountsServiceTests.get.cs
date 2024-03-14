using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAccounts()
        {
            var entitiesMock = this.accountModelBuilder.Generate(10);

            this.provider
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(entitiesMock.ToArray())
                .Verifiable();

            var result = await this.service.GetAllAsync();

            Assert.IsInstanceOf<ServiceResult<ICollection<AccountDto>>>(result);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(entitiesMock.Count, result.Data!.Count);

            this.provider.Verify(x => x.GetAllAsync(),Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_ExistingAccount_ReturnsAccount()
        {
            var id = Guid.NewGuid();
            var entitiesMock = this.accountModelBuilder
                .WithId(id)
                .Generate();

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(ServiceResult.Success);
            this.provider
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(entitiesMock);

            var result = await this.service.GetByIdAsync(id);

            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOf<ServiceResult<AccountDto>>(result);
        }

        [Test]
        public async Task GetByIdAsync_NotExistingAccount_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var entitiesMock = this.accountModelBuilder
                .WithId(id)
                .Generate();
            var expectedErrorMessage = $"Not found Account with id {id}";

            this.validator
                .Setup(x => x.ExistsAsync(id))
                .ReturnsAsync(new NotFoundError(expectedErrorMessage));

            var result = await this.service.GetByIdAsync(id);

            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOf<NotFoundError>(result.Error);
            Assert.AreEqual(expectedErrorMessage, result.Error!.Message);
        }
    }
}
