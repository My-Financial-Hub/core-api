namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        #region GetById
        [Test]
        public async Task GetAsync_ExistingUser_ReturnsUser()
        {
            var expectedUser = fixture.Context.Users.Add(builder.Generate()).Entity;
            fixture.Context.SaveChanges();

            var user = await repository.GetAsync(expectedUser.Id.GetValueOrDefault());

            EntityAssert.Equal(expectedUser, user!);
        }

        [Test]
        public async Task GetAsync_NotExistingUser_ReturnsNull()
        {
            var user = await repository.GetAsync(Guid.NewGuid());

            Assert.That(user, Is.Null);
        }
        #endregion
    }
}
