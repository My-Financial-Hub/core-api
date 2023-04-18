namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        [Test]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            var oldUser = context.Users.Add(builder.Generate()).Entity;

            var newUser = builder.WithId(oldUser.Id.GetValueOrDefault()).Generate();
            var updated = await repository.UpdateAsync(newUser);

            Assert.That(updated, Is.EqualTo(newUser));
        }

        [Test]
        public async Task UpdateAsync_ExistingUser_UpdatesUser() 
        {
            var oldUser = context.Users.Add(builder.Generate()).Entity;

            var newUser = builder.WithId(oldUser.Id.GetValueOrDefault()).Generate();
            await repository.UpdateAsync(newUser);
        }

        [Test]
        public async Task UpdateAsync_NotExistingUser_DoesNotUpdatesUser()
        {
            var newUser = builder.Generate();

            await repository.UpdateAsync(newUser);

            var datebaseUser = context.Users.FirstOrDefault(u => u.Id == newUser.Id);
            Assert.That(datebaseUser, Is.Null);
        }

        [Test]
        public void UpdateAsync_NotExistingUser_ThrowsDbUpdateConcurrencyException()
        {
            var newUser = builder.Generate();
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await this.repository.UpdateAsync(newUser));
        }
    }
}
