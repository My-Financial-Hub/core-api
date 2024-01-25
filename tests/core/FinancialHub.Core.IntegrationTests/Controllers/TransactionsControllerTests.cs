using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.IntegrationTests
{
    public class TransactionsControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder entityBuilder;
        private TransactionModelBuilder modelBuilder;

        public TransactionsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/Transactions")
        {

        }

        public override void SetUp()
        {
            this.entityBuilder = new TransactionEntityBuilder();
            this.modelBuilder = new TransactionModelBuilder(); 
            base.SetUp();
        }

        protected async Task AssertGetExists(TransactionModel expected)
        {
            var getResponse = await this.client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            TransactionModelAssert.Equal(expected, getResult!.Data.First());
        }

        protected TransactionModel CreateValidTransaction(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            this.fixture.AddData(model.Category);
            this.fixture.AddData(model.Balance);

            return modelBuilder
                .WithBalanceId(model.Balance.Id)
                .WithCategoryId(model.Category.Id)
                .WithActiveStatus(isActive)
                .Generate();
        }

        protected TransactionModel InsertTransaction(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            var account     = this.fixture.AddData(model.Balance.Account).First();
            model.Balance.Account = null;
            model.Balance.AccountId = account.Id.GetValueOrDefault();

            var balance     = this.fixture.AddData(model.Balance).First();
            var category    = this.fixture.AddData(model.Category).First();

            var data = entityBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .WithActiveStatus(isActive)
                .Generate();

            data = this.fixture.AddData(data).First();

            return modelBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .WithActiveStatus(isActive)
                .WithId(data.Id.GetValueOrDefault())
                .Generate();
        }

        protected Guid? InsertTransaction(TransactionModel model)
        {
            var oldEntityBuilder = entityBuilder.Generate();
            oldEntityBuilder.BalanceId = model.BalanceId;
            oldEntityBuilder.Balance.Id = model.BalanceId;
            oldEntityBuilder.Balance.AccountId = model.Balance.AccountId;
            oldEntityBuilder.Balance.Account.Id = model.Balance.AccountId;
            oldEntityBuilder.CategoryId = model.CategoryId;
            oldEntityBuilder.Category.Id = model.CategoryId;

            var entity = entityBuilder
                .WithStatus(model.Status)
                .WithType(model.Type)
                .WithTargetDate(model.TargetDate.DateTime)
                .WithAmount(model.Amount)
                .WithBalance(oldEntityBuilder.Balance)
                .WithCategory(oldEntityBuilder.Category)
                .WithActiveStatus(model.IsActive)
                .Generate();

            var account = this.fixture.AddData(entity.Balance.Account)[0];
            entity.Balance.Account = null;
            entity.Balance.AccountId = account.Id.GetValueOrDefault();

            var balance = this.fixture.AddData(entity.Balance)[0];
            var category = this.fixture.AddData(entity.Category)[0];

            var data = entityBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .Generate();

            var transaction = this.fixture.AddData(data);
            return transaction[0].Id;
        }

        protected TransactionModel[] InsertTransactions(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            this.fixture.AddData(model.Category);
            this.fixture.AddData(model.Balance);

            var data = entityBuilder
                .WithBalanceId(model.Balance.Id)
                .WithCategoryId(model.Category.Id)
                .WithActiveStatus(isActive)
                .Generate(10);

            this.fixture.AddData(data.ToArray());

            return data
                .Select(
                    x => modelBuilder
                            .WithBalanceId(x.BalanceId)
                            .WithCategoryId(x.BalanceId)
                            .WithActiveStatus(isActive)
                            .WithId(x.Id.GetValueOrDefault())
                            .Generate()
                ).ToArray();
        }

        [Test]
        public async Task GetAll_ReturnActiveTransactions()
        {
            var data = this.InsertTransactions();

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(data.Length, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_DoesNotReturnNotActiveTransactions()
        {
            this.InsertTransactions(false);

            var response = await this.client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.Zero(result!.Data.Count);
        }

        [Test]
        public async Task Post_ValidTransaction_ReturnCreatedTransaction()
        {
            var data = this.CreateValidTransaction();

            var response = await this.client.PostAsync(baseEndpoint, data);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            TransactionModelAssert.Equal(data, result!.Data);
        }

        [Test]
        public async Task Post_ValidTransaction_CreateTransaction()
        {
            var body = this.CreateValidTransaction();

            await this.client.PostAsync(baseEndpoint, body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_ExistingTransaction_ReturnUpdatedTransaction()
        {
            var data = this.InsertTransaction(true);

            var body = this.modelBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .WithId(data.Id.GetValueOrDefault())
                .Generate();

            var response = await this.client.PutAsync($"{baseEndpoint}/{data.Id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionModel>>();
            Assert.IsNotNull(result?.Data);
            Assert.AreEqual(body.Id, result?.Data.Id);
            TransactionModelAssert.Equal(body,result!.Data);
        }

        [Test]
        public async Task Put_ExistingTransaction_UpdatesTransaction()
        {
            var data = this.InsertTransaction(true);

            var body = this.modelBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .WithId(data.Id.GetValueOrDefault())
                .Generate();
            await this.client.PutAsync($"{baseEndpoint}/{data.Id}", body);

            await this.AssertGetExists(body);
        }

        [Test]
        public async Task Put_NonExistingTransaction_ReturnNotFoundError()
        {
            var body = this.CreateValidTransaction();

            var response = await this.client.PutAsync($"{baseEndpoint}/{body.Id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Transaction with id {body.Id}", result!.Message);
        }

        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var body = this.InsertTransaction();

            var response = await this.client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_PaidTransaction_RemovesTransactionFromDatabase()
        {
            var body = this.modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();

            body.Id = this.InsertTransaction(body);

            await this.client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var result = this.fixture.GetData<TransactionEntity>();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task Delete_EarnPaidTransaction_DecreasesBalanceFromDatabase()
        {
            var body = this.modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Earn)
                .WithActiveStatus(true)
                .Generate();

            body.Id = this.InsertTransaction(body);
            var oldBalanceAmount = this.fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId).Amount;

            await this.client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var balance = this.fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId);
            Assert.That(balance.Amount, Is.EqualTo(oldBalanceAmount - body.Amount));
        }

        [Test]
        public async Task Delete_ExpensePaidTransaction_DecreasesBalanceFromDatabase()
        {
            var body = this.modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Expense)
                .WithActiveStatus(true)
                .Generate();

            body.Id = this.InsertTransaction(body);
            var oldBalanceAmount = this.fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId).Amount;

            await this.client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var balance = this.fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId);
            Assert.That(balance.Amount, Is.EqualTo(oldBalanceAmount + body.Amount));
        }

        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, false)]
        public async Task Delete_NotPaidTransaction_RemovesTransactionFromDatabase(TransactionStatus status, bool isActive)
        {
            var body = this.modelBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .Generate();

            body.Id = this.InsertTransaction(body);

            await this.client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var result = this.fixture.GetData<TransactionEntity>();
            Assert.AreEqual(0, result.Count());
        }
    }
}