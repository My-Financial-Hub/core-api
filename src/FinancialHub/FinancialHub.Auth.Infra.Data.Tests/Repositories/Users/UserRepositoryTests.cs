namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    [TestFixtureSource(typeof(DatabaseFixture))]
    public partial class UserRepositoryTests
    {
        private readonly DatabaseFixture fixture;
        private IUserRepository repository;
        private UserEntityBuilder builder;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [SetUp]
        public void SetUp()
        {
            this.fixture.SetUp();
            this.builder = new UserEntityBuilder();
            this.repository = new UserRepository(this.fixture.Context);
        }

        [TearDown]
        public void TearDown()
        {
            this.fixture.TearDown();
        }
    }
}
