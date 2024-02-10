namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder entityBuilder;
        private TransactionModelBuilder modelBuilder;

        public TransactionsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/Transactions")
        {

        }

        public override void SetUp()
        {
            entityBuilder = new TransactionEntityBuilder();
            modelBuilder = new TransactionModelBuilder();
            base.SetUp();
        }

        protected async Task AssertGetExists(TransactionModel expected)
        {
            var getResponse = await client.GetAsync(baseEndpoint);

            var getResult = await getResponse.ReadContentAsync<ListResponse<TransactionModel>>();
            Assert.AreEqual(1, getResult?.Data.Count);
            TransactionModelAssert.Equal(expected, getResult!.Data.First());
        }

        protected TransactionModel CreateValidTransaction(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            fixture.AddData(model.Category);
            fixture.AddData(model.Balance);

            return modelBuilder
                .WithBalanceId(model.Balance.Id)
                .WithCategoryId(model.Category.Id)
                .WithActiveStatus(isActive)
                .Generate();
        }

        protected TransactionModel InsertTransaction(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            var account = fixture.AddData(model.Balance.Account).First();
            model.Balance.Account = null;
            model.Balance.AccountId = account.Id.GetValueOrDefault();

            var balance = fixture.AddData(model.Balance).First();
            var category = fixture.AddData(model.Category).First();

            var data = entityBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .WithActiveStatus(isActive)
                .Generate();

            data = fixture.AddData(data).First();

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

            var account = fixture.AddData(entity.Balance.Account)[0];
            entity.Balance.Account = null;
            entity.Balance.AccountId = account.Id.GetValueOrDefault();

            var balance = fixture.AddData(entity.Balance)[0];
            var category = fixture.AddData(entity.Category)[0];

            var data = entityBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .Generate();

            var transaction = fixture.AddData(data);
            return transaction[0].Id;
        }

        protected TransactionModel[] InsertTransactions(bool isActive = true)
        {
            var model = entityBuilder.Generate();

            fixture.AddData(model.Category);
            fixture.AddData(model.Balance);

            var data = entityBuilder
                .WithBalanceId(model.Balance.Id)
                .WithCategoryId(model.Category.Id)
                .WithActiveStatus(isActive)
                .Generate(10);

            fixture.AddData(data.ToArray());

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
    }
}