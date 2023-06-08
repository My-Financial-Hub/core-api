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

            ModelAssert.Equal(user, createdUser);
        }

        [Test]
        public void CreateAsync_RepositoryException_ThrowsException()
        {
            var user = this.builder.Generate();
            var exc = new Exception("Exception Message");

            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<UserEntity>()))
                .ThrowsAsync(exc);

            Assert.ThrowsAsync<Exception>(async () => await this.provider.CreateAsync(user));
        }
    }
}
