using FinancialHub.Core.Domain.Interfaces.Repositories;
using FinancialHub.Core.Infra.Data.Repositories;
using FinancialHub.Core.Infra.Data.Tests.Repositories.Base;

namespace FinancialHub.Core.Infra.Data.Tests.Repositories
{
    public partial class BalancesRepositoryTests : BaseRepositoryTests<BalanceEntity>
    {
        public TransactionEntityBuilder transactionBuilder;
        public IBalancesRepository balanceRepository;
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
