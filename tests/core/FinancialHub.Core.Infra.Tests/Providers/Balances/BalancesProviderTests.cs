using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;
using Microsoft.Extensions.Logging;

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
        private Mock<IBalancesCache> cache;
        private Mock<ILogger<BalancesProvider>> logger;

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
            this.cache      = new Mock<IBalancesCache>();
            this.logger     = new Mock<ILogger<BalancesProvider>>();

            this.provider = new BalancesProvider(
                this.repository.Object, this.cache.Object,
                this.mapper,
                this.logger.Object
            );

            this.random = new Random();

            this.accountEntityBuilder   = new AccountEntityBuilder();
            this.balanceEntityBuilder   = new BalanceEntityBuilder();
            this.balanceModelBuilder    = new BalanceModelBuilder();
        }
    }
}
