using FinancialHub.Auth.Domain.Interfaces.Helpers;

namespace FinancialHub.Auth.Infra.Tests.Providers.Credentials
{
    public partial class CredentialProviderTests
    {
        private IMapper mapper;
        private Mock<ICredentialRepository> mockRepository;
        private Mock<IPasswordHelper> passwordHelperMock;
        private ICredentialProvider provider;

        private UserCredentialModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            this.builder = new UserCredentialModelBuilder();

            this.passwordHelperMock = new Mock<IPasswordHelper>();
            this.mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAuthCredentialProfile(this.passwordHelperMock.Object));
            }
            ).CreateMapper();
            this.mockRepository = new Mock<ICredentialRepository>();
            this.provider = new CredentialProvider(this.mockRepository.Object, this.mapper);
        }
    }
}
