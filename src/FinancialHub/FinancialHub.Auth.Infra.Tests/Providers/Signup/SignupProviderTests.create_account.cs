namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class SignupProviderTests
    {
        [Test]
        public async Task CreateAccountAsync_ValidUser_ReturnsCreatedUser()
        {
            var signupModel = builder.Generate();

            var user = mapper.Map<UserModel>(signupModel);
            userProvider
                .Setup(x => x.CreateAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(user);

            var credential = mapper.Map<CredentialModel>(signupModel);
            credentialProvider
                .Setup(x => x.CreateAsync(It.IsAny<CredentialModel>()))
                .ReturnsAsync(credential);

            var createdAccount = await provider.CreateAccountAsync(signupModel);

            Assert.That(createdAccount, Is.Not.Null);
            ModelAssert.Equal(user, createdAccount);
        }

        [Test]
        public async Task CreateAccountAsync_ValidUser_CallsCreateCredentialWithUserId()
        {
            var signupModel = builder.Generate();
            var guid = Guid.NewGuid();

            var user = mapper.Map<UserModel>(signupModel);
            user.Id = guid;
            userProvider
                .Setup(x => x.CreateAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(user);

            passwordHelper
                .Setup(x => x.Encrypt(signupModel.Password))
                .Returns(signupModel.Password);

            var credential = mapper.Map<CredentialModel>(signupModel);
            credentialProvider
                .Setup(x => x.CreateAsync(It.IsAny<CredentialModel>()))
                .ReturnsAsync(credential)
                .Verifiable();

            await provider.CreateAccountAsync(signupModel);

            credentialProvider.Verify(x => x.CreateAsync(It.Is<CredentialModel>(c => c.UserId == guid)), Times.Once);
        }
    }
}
