namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        #region GetById
        [Test]
        public async Task GetAsync_ExistingUser_ReturnsUser()
        {
            var expectedUser = context.Users.Add(builder.Generate()).Entity;

            var user = await repository.GetAsync(expectedUser.Id.GetValueOrDefault());

            Assert.That(user,Is.EqualTo(expectedUser));
        }

        [Test]
        public async Task GetAsync_NotExistingUser_ReturnsNull()
        {
            var user = await repository.GetAsync(Guid.NewGuid());

            Assert.That(user, Is.Null);
        }
        #endregion

        #region GetAll
        [Test]
        public async Task GetAllAsync_ExistingUsers_ReturnsAllUser()
        {
            context.Users.AddRange(builder.Generate(10));

            var users = await repository.GetAllAsync();

            Assert.That(users, Is.EqualTo(context.Users));
        }

        [Test]
        public async Task GetAllAsync_NotExistingUsers_ReturnsEmptyList()
        {
            var user = await repository.GetAllAsync();

            Assert.That(user, Is.Empty);
        }
        #endregion
    }
}
