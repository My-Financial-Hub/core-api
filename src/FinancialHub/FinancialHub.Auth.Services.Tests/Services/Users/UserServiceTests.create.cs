using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Services.Tests.Services
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
            AssertEqual(user, createdUserResult.Data);
            mockProvider.Verify(x => x.CreateAsync(It.IsAny<UserModel>()), Times.Once());
        }

        [Test]
        public void CreateAsync_Exception_ThrowsException()
        {
            var user = this.builder.Generate();
            var exc = new Exception("Exception Message");

            mockProvider
                .Setup(x => x.CreateAsync(It.IsAny<UserModel>()))
                .ThrowsAsync(exc)
                .Verifiable();

            Assert.ThrowsAsync<Exception>(async () => await this.service.CreateAsync(user));
            mockProvider.Verify(x => x.CreateAsync(It.IsAny<UserModel>()), Times.Once());
        }
    }
}
