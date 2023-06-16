namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ValidUser_InsertsUser()
        {
            var user = this.builder.Generate();

            await this.repository.CreateAsync(user);

            DbContextAssert.AssertCreated(fixture.Context, user);
        }

        [Test]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
        {
            var user = this.builder.Generate();

            var result = await this.repository.CreateAsync(user);

            EntityAssert.Equal(user, result);
        }

        [Test]
        public async Task CreateAsync_UserWithExistingId_InsertsIntoDatabaseWithDifferentId()
        {
            var user = this.builder.Generate();

            await this.repository.CreateAsync(user);
            await this.repository.CreateAsync(user);

            DbContextAssert.AssertCreated(fixture.Context, user);
        }
    }
}
