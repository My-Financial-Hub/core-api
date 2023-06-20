namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class UserProviderTests
    {
        private IMapper mapper;
        private Mock<IUserRepository> mockRepository;
        private IUserProvider provider;

        private UserModelBuilder builder;

        [SetUp]
        public void SetUp() 
        {
            this.builder = new UserModelBuilder();

            this.mapper = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new FinancialHubAuthProviderProfile());
                }
            ).CreateMapper();
            this.mockRepository = new Mock<IUserRepository>();
            this.provider = new UserProvider(this.mockRepository.Object, this.mapper);
        }
    }
}
