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
        private Mock<IBalancesRepository> balanceRepository;

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
            this.balanceRepository = new Mock<IBalancesRepository>();
            this.provider = new TransactionsProvider(this.mapper, this.repository.Object, this.balanceRepository.Object);

            this.random = new Random();

            this.transactionBuilder         = new TransactionEntityBuilder();
            this.transactionModelBuilder    = new TransactionModelBuilder();
            this.balanceBuilder             = new BalanceEntityBuilder();
        }
    }
}
