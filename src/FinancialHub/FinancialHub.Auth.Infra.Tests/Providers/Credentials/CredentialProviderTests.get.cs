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

            var user = await this.provider.GetAsync(login);

            Assert.That(user, Is.Null);
            mockRepository.Verify(x => x.GetAsync(login), Times.Once);
        }
    }
}
