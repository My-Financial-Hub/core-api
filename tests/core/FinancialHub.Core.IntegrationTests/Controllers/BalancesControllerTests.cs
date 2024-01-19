namespace FinancialHub.Core.IntegrationTests
{
    public class BalancesControllerTests : BaseControllerTests
    {
        private BalanceEntityBuilder entityBuilder;
        private BalanceModelBuilder modelBuilder;
        private AccountEntityBuilder accountBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public BalancesControllerTests(FinancialHubApiFixture fixture) : base(fixture ,"/balances")
        {
        }

        public override void SetUp()
        {
            this.entityBuilder  = new BalanceEntityBuilder();
            this.modelBuilder   = new BalanceModelBuilder();
            this.accountBuilder = new AccountEntityBuilder();
            this.transactionBuilder = new TransactionEntityBuilder();
            base.SetUp();
        }

        protected void AssertExists(BalanceModel expected)
        {
            var data = this.fixture.GetData<BalanceEntity>();
            BalanceModelAssert.Equal(expected, data.First());
        }

        [Test]
        public async Task Post_BalanceWithInvalidAccount_Returns404NotFound()
        {
            var id = Guid.NewGuid();
            var data = this.modelBuilder.WithAccountId(id).Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual(result?.Message, $"Not found Account with id {id}");
        }

        [Test]
        public async Task Post_BalanceWithInvalidAccount_DoesNotCreateBalance()
        {
            var id = Guid.NewGuid();
            var data = this.modelBuilder.WithAccountId(id).Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var balance = this.fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.AccountId == id);
            Assert.IsNull(balance);
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
        public async Task Delete_ReturnsNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesBalanceFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var result = this.fixture.GetData<BalanceEntity>();
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Delete_RemovesTransactionsFromDatabase()
        {
            var account = accountBuilder.WithId(Guid.NewGuid()).Generate();
            this.fixture.AddData(account);

            var balance = entityBuilder.WithAccountId(account.Id).Generate();
            this.fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            this.fixture.AddData(transaction);

            await this.client.DeleteAsync($"{baseEndpoint}/{balance.Id}");

            var transactions = this.fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == account.Id));
        }

        [Test]
        public async Task Delete_RemovesTransactionsOnlyFromTheSelectedBalanceFromDatabase()
        {
            var account = accountBuilder.WithId(Guid.NewGuid()).Generate();
            this.fixture.AddData(account);

            var balance = entityBuilder.WithAccountId(account.Id).Generate();
            this.fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            this.fixture.AddData(transaction);

            var balance2 = entityBuilder.WithAccountId(account.Id).Generate();
            this.fixture.AddData(balance2);

            var transaction2 = transactionBuilder
                .WithBalanceId(balance2.Id)
                .WithCategoryId(transaction.CategoryId)
                .Generate();
            this.fixture.AddData(transaction2);

            await this.client.DeleteAsync($"{baseEndpoint}/{balance.Id}");

            var transactions = this.fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == account.Id));
            Assert.IsNotEmpty(transactions.Where(x => x.BalanceId == balance2.Id && x.Balance.AccountId == account.Id));
        }
    }
}
