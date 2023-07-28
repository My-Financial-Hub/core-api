using FinancialHub.Auth.Domain.Interfaces.Helpers;

namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class SignupProviderTests
    {
        private IMapper mapper;

        private Mock<ICredentialProvider> credentialProvider;
        private Mock<IUserProvider> userProvider;
        private Mock<IPasswordHelper> passwordHelper;
        private ISignupProvider provider;

        private SignupModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            this.builder = new SignupModelBuilder();

            this.passwordHelper = new Mock<IPasswordHelper>();
            this.mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAuthCredentialProfile(this.passwordHelper.Object));
            }).CreateMapper();

            this.userProvider = new Mock<IUserProvider>();
            this.credentialProvider = new Mock<ICredentialProvider>();
            this.provider = new SignupProvider(this.credentialProvider.Object, this.userProvider.Object, this.mapper);
        }
    }
}
