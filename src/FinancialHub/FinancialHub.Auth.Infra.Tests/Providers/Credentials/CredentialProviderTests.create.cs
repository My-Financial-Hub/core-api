namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class CredentialProviderTests
    {
        [Test]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
        {
            var credential = this.builder.Generate();

            var credentialEntity = this.mapper.Map<CredentialEntity>(credential);
            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<CredentialEntity>()))
                .ReturnsAsync(credentialEntity);

            var createdUser = await this.provider.CreateAsync(credential);

            Assert.That(createdUser, Is.Not.Null);
            ModelAssert.Equal(credential, createdUser);
        }

        [Test]
        public void CreateAsync_RepositoryException_ThrowsException()
        {
            var credential = this.builder.Generate();
            var exc = new Exception("Exception Message");

            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<CredentialEntity>()))
                .ThrowsAsync(exc);

            Assert.ThrowsAsync<Exception>(async () => await this.provider.CreateAsync(credential));
        }
    }
}
