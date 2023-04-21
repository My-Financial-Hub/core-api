using FinancialHub.Auth.Domain.Entities;

namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class UserProviderTests
    {
        [Test]
        public async Task GetByIdAsync_ExistingUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var userModel = this.builder.WithId(id).Generate();

            var userEntity = this.mapper.Map<UserEntity>(userModel);
            mockRepository
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(userEntity);
            var resultUser = await this.provider.GetAsync(id);

            this.AssertEqual(userModel, resultUser);
        }

        [Test]
        public async Task GetByIdAsync_NotExistingUser_ReturnsNull()
        {
            var id = Guid.NewGuid();
            var userModel = this.builder.WithId(id).Generate();

            mockRepository
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync((UserEntity?)default);
            var user = await this.provider.GetAsync(id);

            Assert.That(user, Is.Null);
        }
    }
}
