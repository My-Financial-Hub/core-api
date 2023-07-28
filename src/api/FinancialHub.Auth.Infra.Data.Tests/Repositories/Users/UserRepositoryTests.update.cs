namespace FinancialHub.Auth.Infra.Data.Tests.Repositories
{
    public partial class UserRepositoryTests
    {
        [Test]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            var item = await this.fixture.InsertData(builder.Generate());

            var newUser = builder.WithId(item.Id.GetValueOrDefault()).Generate();
            var updated = await repository.UpdateAsync(newUser);

            EntityAssert.Equal(newUser, updated);
        }

        [Test]
        public async Task UpdateAsync_ExistingUser_UpdatesUser() 
        {
            var oldUser = await this.fixture.InsertData(builder.Generate());

            var newUser = builder.WithId(oldUser.Id.GetValueOrDefault()).Generate();
            await repository.UpdateAsync(newUser);

            var data = fixture.Context.Users.FirstOrDefault(x => x.Id == oldUser.Id.GetValueOrDefault());
            EntityAssert.Equal(newUser, data!);
        }
    }
}
