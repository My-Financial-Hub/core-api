namespace FinancialHub.Core.IntegrationTests.Controllers.Balances
{
    public partial class BalancesControllerTests : BaseControllerTests
    {
        private BalanceEntityBuilder balanceBuilder;
        private AccountEntityBuilder accountBuilder;
        private TransactionEntityBuilder transactionBuilder;

        public BalancesControllerTests(FinancialHubApiFixture fixture) : base(fixture, "/balances")
        {
        }

        public override void SetUp()
        {
            balanceBuilder = new BalanceEntityBuilder();
            accountBuilder = new AccountEntityBuilder();
            transactionBuilder = new TransactionEntityBuilder();
            AddCreateBalanceBuilder();
            AddUpdateBalanceBuilder();
            base.SetUp();
        }
    }
}
