namespace FinancialHub.Core.IntegrationTests.Controllers.Balances
{
    public partial class BalancesControllerTests : BaseControllerTests
    {
        private BalanceEntityBuilder entityBuilder;
        private BalanceModelBuilder modelBuilder;
        private AccountEntityBuilder accountBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public BalancesControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/balances")
        {
        }

        public override void SetUp()
        {
            entityBuilder = new BalanceEntityBuilder();
            modelBuilder = new BalanceModelBuilder();
            accountBuilder = new AccountEntityBuilder();
            transactionBuilder = new TransactionEntityBuilder();
            base.SetUp();
        }

        protected void AssertExists(BalanceModel expected)
        {
            var data = fixture.GetData<BalanceEntity>();
            BalanceModelAssert.Equal(expected, data.First());
        }
    }
}
