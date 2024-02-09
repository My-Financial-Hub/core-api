namespace FinancialHub.Core.IntegrationTests.Controllers.Accounts
{
    public partial class AccountsControllerTests
    {

        [Test]
        public async Task Put_ExistingAccount_ReturnUpdatedAccount()
        {
            var id = Guid.NewGuid();
            fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = modelBuilder.WithId(id).Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AccountModelAssert.Equal(body, result!.Data);
        }

        [Test]
        public async Task Put_ExistingAccount_UpdatesAccount()
        {
            var id = Guid.NewGuid();
            fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = modelBuilder.WithId(id).Generate();
            await client.PutAsync($"{baseEndpoint}/{id}", body);

            await AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingAccount_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = modelBuilder.WithId(id).Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Account with id {id}", result!.Message);
        }
    }
}
