namespace FinancialHub.Core.IntegrationTests.Controllers.Balances
{
    public partial class BalancesControllerTests
    {
        [Test]
        public async Task Put_ExistingBalance_ReturnsUpdatedBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var data = modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.IsNotNull(result?.Data);
            BalanceModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Put_ExistingBalance_UpdatesBalance()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var data = modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            await client.PutAsync($"{baseEndpoint}/{id}", data);

            AssertExists(data);
        }

        [Test]
        public async Task Put_ExistingBalance_DoesNotUpdatesBalanceAmount()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithAmount(0)
                .WithId(id)
                .Generate();
            fixture.AddData(entity);

            var data = modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.Zero(result!.Data.Amount);
        }

        [Test]
        public async Task Put_NotExistingBalance_ReturnsNotFound()
        {
            var account = accountBuilder.Generate();
            fixture.AddData(account);

            var entity = entityBuilder
                .WithAccountId(account.Id)
                .Generate();
            fixture.AddData(entity);

            var id = Guid.NewGuid();
            var data = modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Balance with id {id}");
        }
    }
}
