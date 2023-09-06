namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class UserServiceTests
    {
        [Test]
        public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
        {
            var id = Guid.NewGuid();
            var user = this.builder
                .WithId(id)
                .Generate();

            mockProvider
                .Setup(x => x.GetAsync(id))
                .ReturnsAsync(user);
            mockProvider
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(user);

            var updatedUserResult = await this.service.UpdateAsync(id, user);

            Assert.That(updatedUserResult.HasError, Is.False);
            ModelAssert.Equal(user, updatedUserResult.Data);
        }

        [Test]
        public async Task UpdateAsync_NonExistingUser_ReturnsNotFoundServiceError()
        {
            var id = Guid.NewGuid();
            var user = this.builder
                .WithId(id)
                .Generate();

            mockProvider
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(user);

            var updatedUserResult = await this.service.UpdateAsync(id, user);
            Assert.Multiple(() =>
            {
                Assert.That(updatedUserResult.HasError, Is.True);
                Assert.That(updatedUserResult.Error!.Message, Is.EqualTo("User not found"));
            });
        }
    }
}
