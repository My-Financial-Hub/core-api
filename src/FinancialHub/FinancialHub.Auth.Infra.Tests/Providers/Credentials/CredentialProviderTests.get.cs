namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class CredentialProviderTests
    {
        [Test]
        public async Task GetAsyncByLogin_ExistingCredential_ReturnsCredential()
        {
            var credentialModel = this.builder.Generate();
            var login = credentialModel.Login;
            var credentialEntity = this.mapper.Map<CredentialEntity>(credentialModel);

            mockRepository
                .Setup(x => x.GetAsync(login))
                .ReturnsAsync(credentialEntity)
                .Verifiable();

            var resultUser = await this.provider.GetAsync(login);

            Assert.That(resultUser, Is.Not.Null);
            ModelAssert.Equal(credentialModel, credentialEntity);
            mockRepository.Verify(x => x.GetAsync(login), Times.Once);
        }

        [Test]
        public async Task GetAsyncByLogin_NotExistingCredential_ReturnsNull()
        {
            var credentialModel = this.builder.Generate();
            var login = credentialModel.Login;

            mockRepository
                .Setup(x => x.GetAsync(login))
                .ReturnsAsync((CredentialEntity?)default);

            var credential = await this.provider.GetAsync(login);

            Assert.That(credential, Is.Null);
            mockRepository.Verify(x => x.GetAsync(login), Times.Once);
        }

        [Test]
        public async Task GetByCredentialAsync_ValidEmailAndPassword_ReturnsFullCredential()
        {
            var credentialModel = this.builder.Generate();
            var credentialEntity = this.mapper.Map<CredentialEntity>(credentialModel);

            mockRepository
                .Setup(x => x.GetAsync(credentialModel.Login, credentialModel.Password))
                .ReturnsAsync(credentialEntity)
                .Verifiable();
            var resultUser = await this.provider.GetAsync(credentialModel);

            Assert.That(resultUser, Is.Not.Null);
            ModelAssert.Equal(credentialModel, resultUser);
            mockRepository.Verify(x => x.GetAsync(credentialModel.Login, credentialModel.Password), Times.Once);
        }

        [Test]
        public async Task GetByCredentialAsync_NotExistingCredential_ReturnsFullCredential()
        {
            var credentialModel = this.builder.Generate();

            mockRepository
                .Setup(x => x.GetAsync(credentialModel.Login, credentialModel.Password))
                .ReturnsAsync((CredentialEntity?)default);

            var credential = await this.provider.GetAsync(credentialModel);

            Assert.That(credential, Is.Null);
        }
    }
}
