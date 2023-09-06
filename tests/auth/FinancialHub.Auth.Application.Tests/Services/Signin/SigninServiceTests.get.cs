namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class SigninServiceTests
    {
        [Test]
        public async Task AuthenticateAsync_ExistingUser_ReturnsToken()
        {
            var login = signinModelBuilder.Generate();
            var user = userBuilder
                .WithEmail(login.Email)
                .Generate();
            var tokenModel = tokenModelBuilder.Generate();

            mockSigninProvider
                .Setup(x => x.GetAccountAsync(login))
                .ReturnsAsync(user);
            mockTokenProvider
                .Setup(x => x.GenerateToken(user))
                .Returns(tokenModel);

            var tokenResult = await this.service.AuthenticateAsync(login);

            Assert.Multiple(() =>
            {
                Assert.That(tokenResult.HasError, Is.False);
                Assert.That(tokenResult.Data, Is.EqualTo(tokenModel));
            });
        }

        [Test]
        public async Task AuthenticateAsync_CallsGetAccountAsync()
        {
            var login = signinModelBuilder.Generate();
            var user = userBuilder
                .WithEmail(login.Email)
                .Generate();
            var tokenModel = tokenModelBuilder.Generate();

            mockSigninProvider
                .Setup(x => x.GetAccountAsync(login))
                .ReturnsAsync(user)
                .Verifiable();
            mockTokenProvider
                .Setup(x => x.GenerateToken(user))
                .Returns(tokenModel)
                .Verifiable();

            await this.service.AuthenticateAsync(login);

            mockSigninProvider.Verify(x => x.GetAccountAsync(login), Times.Once);
        }

        [Test]
        public async Task AuthenticateAsync_NotExistingUser_ReturnsNull()
        {
            var login = signinModelBuilder.Generate(); 
            
            mockSigninProvider
                .Setup(x => x.GetAccountAsync(login))
                .ReturnsAsync(default(UserModel?));
            var tokenResult = await this.service.AuthenticateAsync(login);

            Assert.Multiple(() =>
            {
                Assert.That(tokenResult.HasError, Is.True);
                Assert.That(tokenResult.Error!.Message, Is.EqualTo("Wrong e-mail or password"));
            });
        }
    }
}
