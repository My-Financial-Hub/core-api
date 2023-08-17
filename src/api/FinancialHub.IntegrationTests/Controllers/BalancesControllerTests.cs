namespace FinancialHub.IntegrationTests
{
    public class BalancesControllerTests : BaseControllerTests
    {
        private BalanceEntityBuilder entityBuilder;
        private BalanceModelBuilder modelBuilder;
        private AccountEntityBuilder accountBuilder;

        public BalancesControllerTests(FinancialHubApiFixture fixture) : base(fixture ,"/balances")
        {
        }

        public override void SetUp()
        {
            this.entityBuilder  = new BalanceEntityBuilder();
            this.modelBuilder   = new BalanceModelBuilder();
            this.accountBuilder = new AccountEntityBuilder();
            base.SetUp();
        }

        protected void AssertExists(BalanceModel expected)
        {
            var data = this.fixture.GetData<BalanceEntity>();
            BalanceModelAssert.Equal(expected, data.First());
        }

        [Test]
        public async Task Post_ValidBalance_ReturnsCreatedBalance()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);
            var data = this.modelBuilder.WithAccountId(account.Id).Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.IsNotNull(result?.Data);
            BalanceModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidBalance_CreatesBalance()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);
            var body = this.modelBuilder.WithAccountId(account.Id).Generate();

            await this.client.PostAsync(baseEndpoint, body);

            this.AssertExists(body);
        }

        [Test]
        public async Task Post_ValidBalance_CreatesBalanceWithAmountZero()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);
            var body = this.modelBuilder.WithAccountId(account.Id).Generate();

            var response = await this.client.PostAsync(baseEndpoint, body);
            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.Zero(result!.Data.Amount);
        }

        [Test]
        public async Task Put_ExistingBalance_ReturnsUpdatedBalance()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            this.fixture.AddData(entity);

            var data = this.modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.IsNotNull(result?.Data);
            BalanceModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Put_ExistingBalance_UpdatesBalance()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();
            this.fixture.AddData(entity);

            var data = this.modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            await this.client.PutAsync($"{baseEndpoint}/{id}", data);

            this.AssertExists(data);
        }

        [Test]
        public async Task Put_ExistingBalance_DoesNotUpdatesBalanceAmount()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);

            var id = Guid.NewGuid();
            var entity = entityBuilder
                .WithAccountId(account.Id)
                .WithAmount(0)
                .WithId(id)
                .Generate();
            this.fixture.AddData(entity);

            var data = this.modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<BalanceModel>>();
            Assert.Zero(result!.Data.Amount);
        }

        [Test]
        public async Task Put_NotExistingBalance_ReturnsNotFound()
        {
            var account = this.accountBuilder.Generate();
            this.fixture.AddData(account);

            var entity = entityBuilder
                .WithAccountId(account.Id)
                .Generate();
            this.fixture.AddData(entity);

            var id = Guid.NewGuid();
            var data = this.modelBuilder
                .WithAccountId(account.Id)
                .WithId(id)
                .Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Balance with id {id}");
        }

        [Test]
        [Ignore("Endpoint disabled")]
        public async Task Delete_ReturnsNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        [Ignore("Endpoint disabled")]
        public async Task Delete_RemovesBalanceFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var result = this.fixture.GetData<BalanceEntity>();
            Assert.IsEmpty(result);
        }
    }
}
