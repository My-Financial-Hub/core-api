using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Services.Services;
using FinancialHub.Auth.Tests.Common.Builders.Models;

namespace FinancialHub.Auth.Services.Tests.Services
{
    public partial class UserServiceTests
    {
        private IUserService service;
        private Mock<IUserProvider> mockProvider;
        private UserModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            this.builder = new UserModelBuilder();

            this.mockProvider = new Mock<IUserProvider>();
            this.service = new UserService(this.mockProvider.Object);
        }
    }
}
