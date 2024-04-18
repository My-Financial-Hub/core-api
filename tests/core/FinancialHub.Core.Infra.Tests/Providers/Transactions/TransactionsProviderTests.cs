using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class TransactionsProviderTests
    {
        protected Random random;
        protected BalanceEntityBuilder  balanceBuilder;

        protected TransactionEntityBuilder  transactionBuilder;
        protected TransactionModelBuilder   transactionModelBuilder;

        private ITransactionsProvider provider;

        private IMapper mapper;
        private Mock<ITransactionsRepository> repository;
        private Mock<IBalancesProvider> balancesProvider;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAutoMapperProfile());
            }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository = new Mock<ITransactionsRepository>();
            this.balancesProvider = new Mock<IBalancesProvider>();
            this.provider = new TransactionsProvider(this.repository.Object, this.balancesProvider.Object, this.mapper);

            this.random = new Random();

            this.transactionBuilder         = new TransactionEntityBuilder();
            this.transactionModelBuilder    = new TransactionModelBuilder();
            this.balanceBuilder             = new BalanceEntityBuilder();
        }
    }
}
