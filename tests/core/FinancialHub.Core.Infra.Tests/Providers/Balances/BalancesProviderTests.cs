using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class BalancesProviderTests
    {
        protected Random random;
        protected AccountEntityBuilder accountEntityBuilder;
        protected BalanceEntityBuilder balanceEntityBuilder;
        protected BalanceModelBuilder  balanceModelBuilder;

        private IBalancesProvider provider;

        private IMapper mapper;
        private Mock<IBalancesRepository> repository;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new FinancialHubAutoMapperProfile());
                }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository = new Mock<IBalancesRepository>();
            this.provider = new BalancesProvider(this.mapper, this.repository.Object);

            this.random = new Random();

            this.accountEntityBuilder   = new AccountEntityBuilder();
            this.balanceEntityBuilder   = new BalanceEntityBuilder();
            this.balanceModelBuilder    = new BalanceModelBuilder();
        }
    }
}
