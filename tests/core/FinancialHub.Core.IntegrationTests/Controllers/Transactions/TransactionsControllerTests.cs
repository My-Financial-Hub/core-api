namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class TransactionsControllerTests : BaseControllerTests
    {
        private TransactionEntityBuilder transactionBuilder;

        public TransactionsControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/Transactions")
        {

        }

        public override void SetUp()
        {
            transactionBuilder = new TransactionEntityBuilder();
            AddCreateTransactionBuilder();
            AddUpdateTransactionBuilder();
            base.SetUp();
        }

        protected TransactionEntity InsertTransaction(bool isActive = true)
        {
            var model = transactionBuilder.Generate();

            var account = fixture.AddData(model.Balance.Account).First();
            model.Balance.Account = null;
            model.Balance.AccountId = account.Id.GetValueOrDefault();

            var balance = fixture.AddData(model.Balance).First();
            var category = fixture.AddData(model.Category).First();

            var data = transactionBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .WithActiveStatus(isActive)
                .Generate();

            return fixture.AddData(data).First();
        }

        protected Guid? InsertTransaction(TransactionEntity model)
        {
            var oldEntityBuilder = transactionBuilder.Generate();
            oldEntityBuilder.BalanceId = model.BalanceId;
            oldEntityBuilder.Balance.Id = model.BalanceId;
            oldEntityBuilder.Balance.AccountId = model.Balance.AccountId;
            oldEntityBuilder.Balance.Account.Id = model.Balance.AccountId;
            oldEntityBuilder.CategoryId = model.CategoryId;
            oldEntityBuilder.Category.Id = model.CategoryId;

            var entity = transactionBuilder
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

            var data = transactionBuilder
                .WithBalanceId(balance.Id)
                .WithCategoryId(category.Id)
                .Generate();

            var transaction = fixture.AddData(data);
            return transaction[0].Id;
        }
    }
}