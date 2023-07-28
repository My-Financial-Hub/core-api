namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class UserServiceTests
    {
        [Test]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser() 
        { 
            var user = this.builder.Generate();

            mockProvider
                .Setup(x => x.CreateAsync(user))
                .ReturnsAsync(user)
                .Verifiable();
            var createdUserResult = await this.service.CreateAsync(user);

            Assert.That(createdUserResult.HasError, Is.False);
            ModelAssert.Equal(user, createdUserResult.Data);
            mockProvider.Verify(x => x.CreateAsync(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
