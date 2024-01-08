namespace FinancialHub.Core.IntegrationTests
{
    public class AccountsControllerTests : BaseControllerTests
    {
        private AccountEntityBuilder entityBuilder;
        private AccountModelBuilder modelBuilder;
        private BalanceEntityBuilder balanceBuilder;
        private CategoryEntityBuilder categoryBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public AccountsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/accounts")
        {

        }

        public override void SetUp()
        {
            this.modelBuilder       = new AccountModelBuilder(); 
            this.entityBuilder      = new AccountEntityBuilder();
            this.balanceBuilder     = new BalanceEntityBuilder();
            this.categoryBuilder    = new CategoryEntityBuilder();
            this.transactionBuilder = new TransactionEntityBuilder();
            base.SetUp();
        }

        protected async Task AssertGetExists(AccountModel expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            AccountModelAssert.Equal(expected, getResult!.Data.First());
        }

        protected void Populate(int amount = 10)
        {
            var account = this.entityBuilder.Generate();
            var data = this.balanceBuilder.WithAccountId(account.Id).Generate(amount);
            this.fixture.AddData(account);
            this.fixture.AddData(data.ToArray());
        }

        protected void PopulateAll()
        {
            var accountAmount = this.random.Next(1, 10);
            var accounts = new AccountEntity[accountAmount];

            for (int i = 0; i < accountAmount; i++)
            {
                accounts[i] = this.entityBuilder.Generate();

                var balanceAmount = this.random.Next(1, 10);
                var balances = this.balanceBuilder.WithAccountId(accounts[i].Id).Generate(balanceAmount);
                this.fixture.AddData(balances.ToArray());
            }

            this.fixture.AddData(accounts);
        }

        [Test]
        public async Task GetAccountsBalances_ReturnBalances()
        {
            var expectedAmount = this.random.Next(1, 10);
            this.Populate(this.random.Next(1, 10));

            var account = this.entityBuilder.Generate();
            var data = this.balanceBuilder.WithAccountId(account.Id).Generate(expectedAmount);
            this.fixture.AddData(account);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync($"{baseEndpoint}/{account.Id}/balances");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<BalanceModel>>();
            Assert.AreEqual(expectedAmount, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_ReturnAccounts()
        {
            var data = entityBuilder.Generate(10);
            this.fixture.AddData(data.ToArray());

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<AccountModel>>();
            Assert.AreEqual(data.Count, result?.Data.Count);
        }

        [Test]
        public async Task Post_ValidAccount_ReturnCreatedAccount()
        {
            var data = this.modelBuilder.Generate();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            AccountModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidAccount_CreateAccount()
        {
            var body = this.modelBuilder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Post_ValidAccount_CreateDefaultBalance()
        {
            var body = this.modelBuilder.Generate();

            await this.client.PostAsync(baseEndpoint, body);

            var account = this.fixture.GetData<AccountEntity>().First();
            var balances = this.fixture.GetData<BalanceEntity>();

            Assert.IsNotNull(balances.FirstOrDefault(x => x.AccountId == account.Id && x.Name == $"{account.Name} Default Balance"));
        }

        [Test]
        public async Task Put_ExistingAccount_ReturnUpdatedAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<AccountModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            AccountModelAssert.Equal(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingAccount_UpdatesAccount()
        {
            var id = Guid.NewGuid();
            this.fixture.AddData(entityBuilder.WithId(id).Generate());

            var body = this.modelBuilder.WithId(id).Generate();
            await this.client.PutAsync($"{baseEndpoint}/{id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingAccount_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = this.modelBuilder.WithId(id).Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Account with id {id}", result!.Message);
        }

        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_RemovesAccountFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            Assert.IsEmpty(this.fixture.GetData<AccountEntity>());
        }

        [Test]
        public async Task Delete_RemovesBalancesFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);
            this.fixture.AddData(balanceBuilder.WithAccountId(id).Generate());

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var balances = this.fixture.GetData<BalanceEntity>();
            Assert.IsEmpty(balances.Where(x => x.AccountId == id));
        }

        [Test]
        public async Task Delete_RemovesTransactionsFromDatabase()
        {
            var id = Guid.NewGuid();

            var data = entityBuilder.WithId(id).Generate();
            this.fixture.AddData(data);

            var balance = balanceBuilder.WithAccountId(id).Generate();
            this.fixture.AddData(balance);

            var transaction = transactionBuilder.WithBalanceId(balance.Id).Generate();
            this.fixture.AddData(transaction);

            await this.client.DeleteAsync($"{baseEndpoint}/{id}");

            var transactions = this.fixture.GetData<TransactionEntity>();
            Assert.IsEmpty(transactions.Where(x => x.BalanceId == balance.Id && x.Balance.AccountId == id));
        }
    }
}