using FinancialHub.Infra.Data.Repositories;
using FinancialHub.Infra.Data.NUnitTests.Repositories.Base;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class BalancesRepositoryTests : BaseRepositoryTests<BalanceEntity>
    {
        public TransactionEntityBuilder transactionBuilder;
        public BalancesRepository balanceRepository;
        public BalanceEntityBuilder balanceBuilder;

        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.balanceRepository  = new BalancesRepository(this.context);
            this.balanceBuilder     = new BalanceEntityBuilder();
            this.transactionBuilder = new TransactionEntityBuilder();

            this.repository         = this.balanceRepository;
            this.builder            = this.balanceBuilder;
        }
    }
}
