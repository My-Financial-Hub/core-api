using FinancialHub.Auth.Domain.Entities;

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

            AssertEqual(user, updatedUser);
        }

        [Test]
        public void UpdateAsync_RepositoryException_ThrowsException()
        {
            var user = this.builder.Generate();
            var exc = new Exception("Exception Message");

            mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<UserEntity>()))
                .ThrowsAsync(exc);

            Assert.ThrowsAsync<Exception>(async () => await this.provider.UpdateAsync(user));
        }
    }
}
