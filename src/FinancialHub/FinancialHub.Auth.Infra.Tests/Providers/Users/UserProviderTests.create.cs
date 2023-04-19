using FinancialHub.Auth.Domain.Entities;

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

            Assert.Multiple(() =>
            {
                Assert.That(createdUser.Id, Is.EqualTo(user.Id));
                Assert.That(createdUser.FirstName, Is.EqualTo(user.FirstName));
                Assert.That(createdUser.LastName, Is.EqualTo(user.LastName));
                Assert.That(createdUser.Email, Is.EqualTo(user.Email));
                Assert.That(createdUser.BirthDate, Is.EqualTo(user.BirthDate));
            });
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
