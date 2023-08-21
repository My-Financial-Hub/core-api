namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class SigninProviderTests
    {
        [Test]
        public async Task GetAccountAsync_ExistingCredential_ReturnsExistingUser()
        {
            var signin = builder.Generate();
            var existingUser = userBuilder
                .WithEmail(signin.Email)
                .Generate();
            var credential = credentialBuilder
                .WithLogin(signin.Email)
                .WithPassword(signin.Password)
                .WithUserId(existingUser.Id)
                .Generate();

            credentialProvider
                .Setup(x => x.GetAsync(It.Is<CredentialModel>(x => x.Login == signin.Email)))
                .ReturnsAsync(credential);
            userProvider
                .Setup(x => x.GetAsync(existingUser.Id.GetValueOrDefault()))
                .ReturnsAsync(existingUser);

            var result = await provider.GetAccountAsync(signin);

            Assert.That(result, Is.Not.Null);
            ModelAssert.Equal(existingUser, result);
        }

        [Test]
        public async Task GetAccountAsync_NotExistingCredential_ReturnsNull()
        {
            var signin = builder.Generate();
            credentialProvider
                .Setup(x => x.GetAsync(It.Is<CredentialModel>(x => x.Login == signin.Email)))
                .ReturnsAsync(default(CredentialModel?))
                .Verifiable();
            var result = await provider.GetAccountAsync(signin);

            Assert.That(result, Is.Null);
            credentialProvider.Verify(x => x.GetAsync(It.Is<CredentialModel>(x => x.Login == signin.Email)), Times.Once);
        }
    }
}
