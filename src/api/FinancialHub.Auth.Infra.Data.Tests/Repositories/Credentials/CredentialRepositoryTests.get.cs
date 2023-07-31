namespace FinancialHub.Auth.Infra.Data.Tests.Repositories.Credentials
{
    public partial class CredentialRepositoryTests
    {
        [Test]
        public async Task GetAsyncByLogin_ExistingCredential_ReturnsCredential()
        {
            var user = await this.fixture.InsertData(userBuilder.Generate());
            var credential = builder
                .WithUserId(user.Id)
                .Generate();
            await this.fixture.InsertData(credential);

            var result = await this.repository.GetAsync(credential.Login);

            Assert.That(result, Is.Not.Null);
            EntityAssert.Equal(credential, result);
        }

        [Test]
        public async Task GetAsyncByLogin_NotExistingCredential_ReturnsNull()
        {
            var user = await this.fixture.InsertData(userBuilder.Generate());
            var credential = builder
                .WithUserId(user.Id)
                .Generate();

            var result = await this.repository.GetAsync(credential.Login);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAsyncByLoginAndPassword_ExistingCredential_ReturnsCredential()
        {
            var user = await this.fixture.InsertData(userBuilder.Generate());
            var credential = builder
                .WithUserId(user.Id)
                .Generate();
            await this.fixture.InsertData(credential);

            var result = await this.repository.GetAsync(credential.Login, credential.Password);

            Assert.That(result, Is.Not.Null);
            EntityAssert.Equal(credential, result);
        }

        [Test]
        public async Task GetAsyncByLoginAndPassword_NotExistingCredential_ReturnsNull()
        {
            var user = await this.fixture.InsertData(userBuilder.Generate());
            var credential = builder
                .WithUserId(user.Id)
                .Generate();

            var result = await this.repository.GetAsync(credential.Login, credential.Password);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetAsyncByUserId_ExistingCredential_ReturnsCredential()
        {
            var expectedCount = fixture.Random.Next(1,10);
            var user = await this.fixture.InsertData(userBuilder.Generate());

            var credentials = builder
                .WithUserId(user.Id)
                .Generate(expectedCount);
            await this.fixture.InsertData(credentials);

            var result = await this.repository.GetAsync(user.Id.GetValueOrDefault());

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetAsyncByUserId_NotExistingCredential_ReturnsEmptyArray()
        {
            var user = await this.fixture.InsertData(userBuilder.Generate());

            var result = await this.repository.GetAsync(user.Id.GetValueOrDefault());

            Assert.That(result, Is.Empty);
        }
    }
}
