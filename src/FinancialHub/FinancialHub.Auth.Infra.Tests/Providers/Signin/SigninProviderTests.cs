using FinancialHub.Auth.Domain.Interfaces.Helpers;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Infra.Tests.Providers.Signin
{
    public partial class SigninProviderTests
    {
        private IMapper mapper;

        private Mock<ICredentialProvider> credentialProvider;
        private Mock<IUserProvider> userProvider;
        private Mock<IPasswordHelper> passwordHelper;
        private ISigninProvider provider;

        private SigninModelBuilder builder;
        private UserCredentialModelBuilder credentialBuilder;
        private UserModelBuilder userBuilder;

        [SetUp]
        public void SetUp()
        {
            this.builder = new SigninModelBuilder();
            this.credentialBuilder = new UserCredentialModelBuilder();
            this.userBuilder = new UserModelBuilder();

            this.passwordHelper = new Mock<IPasswordHelper>();
            this.passwordHelper
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns<string>(x => x);

            this.mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAuthCredentialProfile(this.passwordHelper.Object));
            }).CreateMapper();

            this.userProvider = new Mock<IUserProvider>();
            this.credentialProvider = new Mock<ICredentialProvider>();
            this.provider = new SigninProvider(this.credentialProvider.Object, this.userProvider.Object, this.mapper);
        }
    }
}
