namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ValidUser_InsertsUser()
        {
            var user = this.builder.Generate();

            await this.repository.CreateAsync(user);

            AssertCreated(user);
        }

        [Test]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
        {
            var user = this.builder.Generate();

            var result = await this.repository.CreateAsync(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
                Assert.That(result.LastName, Is.EqualTo(user.LastName));
                Assert.That(result.Email, Is.EqualTo(user.Email));
                Assert.That(result.BirthDate, Is.EqualTo(user.BirthDate));
            });
        }

        [Test]
        public async Task CreateAsync_UserWithExistingId_InsertsIntoDatabaseWithDifferentId()
        {
            var user = this.builder.Generate();

            await this.repository.CreateAsync(user);
            await this.repository.CreateAsync(user);

            AssertCreated(user);
        }
    }
}
