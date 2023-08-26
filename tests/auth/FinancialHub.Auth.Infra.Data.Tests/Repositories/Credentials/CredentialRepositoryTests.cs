namespace FinancialHub.Auth.Infra.Data.Tests.Repositories.Credentials
{
    [TestFixtureSource(typeof(DatabaseFixture))]
    public partial class CredentialRepositoryTests
    {
        private readonly DatabaseFixture fixture;
        private ICredentialRepository repository;
        private UserCredentialEntityBuilder builder;
        private UserEntityBuilder userBuilder;

        public CredentialRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [SetUp]
        public void SetUp()
        {
            this.fixture.SetUp();
            this.builder = new UserCredentialEntityBuilder();
            this.userBuilder = new UserEntityBuilder();
            this.repository = new CredentialRepository(this.fixture.Context);
        }

        [TearDown]
        public void TearDown()
        {
            this.fixture.TearDown();
        }
    }
}
