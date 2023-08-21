namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class UserProviderTests
    {
        [Test]
        public async Task UpdateAsync_ValidUser_ReturnsUpdatedUser()
        {
            var user = this.builder.Generate();

            var userEntity = this.mapper.Map<UserEntity>(user);
            mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(userEntity);

            var updatedUser = await this.provider.UpdateAsync(user);

            ModelAssert.Equal(user, updatedUser);
        }
    }
}
