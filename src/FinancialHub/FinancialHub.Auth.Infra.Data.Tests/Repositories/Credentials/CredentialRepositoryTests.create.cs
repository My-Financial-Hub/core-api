namespace FinancialHub.Auth.Infra.Data.Tests.Repositories.Credentials
{
    public partial class CredentialRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ValidCredeential_InsertsCredential()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUserId(user.Id).Generate();

            await this.repository.CreateAsync(credential);

            DbContextAssert.AssertCreated(this.fixture.Context, credential);
        }

        [Test]
        public async Task CreateAsync_ValidCredential_ReturnsCreatedCredential()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUserId(user.Id).Generate();

            var result = await this.repository.CreateAsync(credential);

            Assert.That(result, Is.Not.Null);
            EntityAssert.Equal(credential, result);
        }

        [Test]
        public async Task CreateAsync_CredentialWithExistingId_InsertsIntoDatabaseWithDifferentId()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUserId(user.Id).Generate();

            await this.repository.CreateAsync(credential);
            await this.repository.CreateAsync(credential);

            DbContextAssert.AssertCreated(this.fixture.Context, credential);
        }
    }
}
