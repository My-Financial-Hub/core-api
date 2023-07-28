using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Application.Services;

namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class SignupServiceTests
    {
        private ISignupService service;
        private Mock<ICredentialProvider> mockCredentialProvider;
        private Mock<ISignupProvider> mockSignupProvider;

        private UserModelBuilder userBuilder;
        private UserCredentialModelBuilder userCredentialBuilder;
        private SignupModelBuilder signupBuilder;

        [SetUp]
        public void SetUp()
        {
            this.userBuilder = new UserModelBuilder();
            this.signupBuilder = new SignupModelBuilder();
            this.userCredentialBuilder = new UserCredentialModelBuilder();

            this.mockCredentialProvider = new Mock<ICredentialProvider>();
            this.mockSignupProvider = new Mock<ISignupProvider>();

            this.service = new SignupService(mockSignupProvider.Object, mockCredentialProvider.Object);
        }
    }
}
