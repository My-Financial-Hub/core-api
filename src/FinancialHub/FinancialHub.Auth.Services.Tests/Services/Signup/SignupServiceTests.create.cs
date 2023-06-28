namespace FinancialHub.Auth.Services.Tests.Services
{
    public partial class SignupServiceTests
    {
        [Test]
        public async Task CreateAccountAsync_ValidSignup_ReturnsCreatedUser()
        {
            var signup = signupBuilder.Generate();
            var user = userBuilder
                .WithEmail(signup.Email)
                .WithName(signup.FirstName)
                .WithLastName(signup.LastName)
                .WithBirthDate(signup.BirthDate)
                .Generate();

            this.mockCredentialProvider
                .Setup(x => x.GetAsync(signup.Email))
                .ReturnsAsync((CredentialModel?)default);
            this.mockSignupProvider
                .Setup(x => x.CreateAccountAsync(signup))
                .ReturnsAsync(user);

            var result = await this.service.CreateAccountAsync(signup);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.HasError, Is.False);
                Assert.That(result.Data, Is.EqualTo(user));
            });
        }

        [Test]
        public async Task CreateAccountAsync_ExistingCredential_ReturnsErrror()
        {
            var signup = signupBuilder.Generate();
            var credential = userCredentialBuilder
                .WithLogin(signup.Email)
                .WithPassword(signup.Password)
                .Generate();

            this.mockCredentialProvider
                .Setup(x => x.GetAsync(signup.Email))
                .ReturnsAsync(credential);

            var result = await this.service.CreateAccountAsync(signup);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.HasError, Is.True);
                Assert.That(result.Error.Message, Is.EqualTo("Credential already exists"));
            });
        }

        [Test]
        public async Task CreateAccountAsync_FailedToCreateUser_ReturnsError()
        {
            var signup = signupBuilder.Generate();

            this.mockCredentialProvider
                .Setup(x => x.GetAsync(signup.Email))
                .ReturnsAsync((CredentialModel?)default);
            this.mockSignupProvider
                .Setup(x => x.CreateAccountAsync(signup))
                .ReturnsAsync((UserModel?)default);

            var result = await this.service.CreateAccountAsync(signup);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.HasError, Is.True);
                Assert.That(result.Error.Message, Is.EqualTo("Failed to create user"));
            });
        }
    }
}
