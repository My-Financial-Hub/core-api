using FinancialHub.Core.Domain.DTOS.Accounts;
using System.Linq;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        [Test]
        [TestCase(Description = "Get by user sucess return",Category = "Get")]
        public async Task GetByUsersAsync_ValidUser_ReturnsAccounts()
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
