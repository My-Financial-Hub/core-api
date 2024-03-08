using FinancialHub.Core.Domain.DTOS.Accounts;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        public async Task GetAllAsync_ValidUser_ReturnsAccounts()
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
    }
}
