﻿namespace FinancialHub.Auth.Infra.Data.Tests.Repositories.Credentials
{
    public partial class CredentialRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ValidCredeential_InsertsCredential()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUser(user).Generate();

            await this.repository.CreateAsync(credential);

            DbContextAssert.AssertCreated(this.fixture.Context, credential);
        }

        [Test]
        public async Task CreateAsync_ValidCredential_ReturnsCreatedCredential()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUser(user).Generate();

            var result = await this.repository.CreateAsync(credential);

            EntityAssert.Equal(credential, result);
        }

        [Test]
        public async Task CreateAsync_CredentialWithExistingId_InsertsIntoDatabaseWithDifferentId()
        {
            var user = await this.fixture.InsertData(this.userBuilder.Generate());
            var credential = this.builder.WithUser(user).Generate();

            await this.repository.CreateAsync(credential);
            await this.repository.CreateAsync(credential);

            DbContextAssert.AssertCreated(this.fixture.Context, credential);
        }

        [Test]
        public void CreateAsync_UserNotCreated_ShouldThrowDbUpdateException()
        {
            var credential = this.builder.Generate();

            Assert.ThrowsAsync<DbUpdateException>( 
                async () => await this.repository.CreateAsync(credential)
            );
        }
    }
}
