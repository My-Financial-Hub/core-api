namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class UserProviderTests
    {
        [Test]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
        {
            var user = this.builder.Generate();

            var userEntity = this.mapper.Map<UserEntity>(user);
            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(userEntity);

            var createdUser = await this.provider.CreateAsync(user);

            Assert.That(createdUser, Is.Not.Null);
            ModelAssert.Equal(user, createdUser);
        }
    }
}
