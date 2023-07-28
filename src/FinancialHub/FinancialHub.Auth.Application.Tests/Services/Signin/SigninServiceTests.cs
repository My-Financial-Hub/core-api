using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Application.Services;

namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class SigninServiceTests
    {
        private ISigninService service;
        private Mock<ITokenService> mockTokenProvider;
        private Mock<ISigninProvider> mockSigninProvider;

        private UserModelBuilder userBuilder;
        private SigninModelBuilder signinModelBuilder;
        private TokenModelBuilder tokenModelBuilder;

        [SetUp]
        public void SetUp()
        {
            this.userBuilder = new UserModelBuilder();
            this.signinModelBuilder = new SigninModelBuilder();
            this.tokenModelBuilder = new TokenModelBuilder();

            this.mockTokenProvider = new Mock<ITokenService>();
            this.mockSigninProvider = new Mock<ISigninProvider>();

            this.service = new SigninService(mockTokenProvider.Object, mockSigninProvider.Object);
        }
    }
}
