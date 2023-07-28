namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class UserServiceTests
    {
        [Test]
        public async Task GetAsync_ExistingUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var user = this.builder.Generate();

            mockProvider
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(user)
                .Verifiable();

            var userResult = await this.service.GetAsync(id);

            Assert.That(userResult.HasError, Is.False);
            ModelAssert.Equal(user, userResult.Data);
        }

        [Test]
        public async Task GetAsync_NotExistingUser_ReturnsNotFoundUser()
        {
            var id = Guid.NewGuid();
            var user = this.builder.Generate();

            mockProvider
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(default(UserModel))
                .Verifiable();

            var userResult = await this.service.GetAsync(id);
            Assert.Multiple(() =>
            {
                Assert.That(userResult.HasError, Is.True);
                Assert.That(userResult.Error.Message, Is.EqualTo("User not found"));
            });
        }

    }
}
