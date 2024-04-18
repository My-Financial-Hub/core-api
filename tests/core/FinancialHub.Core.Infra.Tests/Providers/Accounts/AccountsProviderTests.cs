using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        protected Random random;
        protected AccountEntityBuilder  accountEntityBuilder;
        protected AccountModelBuilder   accountModelBuilder;

        private IAccountsProvider provider;

        private IMapper mapper;
        private Mock<IAccountsRepository> repository;
        private Mock<IAccountsCache> cache;
        private Mock<IBalancesProvider> balancesProvider;
        private Mock<ILogger<AccountsProvider>> mockLogger;

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

            this.repository         = new Mock<IAccountsRepository>();
            this.balancesProvider   = new Mock<IBalancesProvider>();
            this.cache              = new Mock<IAccountsCache>();
            this.mockLogger         = new Mock<ILogger<AccountsProvider>>();
            this.provider = new AccountsProvider(
                this.repository.Object, this.cache.Object,
                this.balancesProvider.Object,
                this.mapper,
                this.mockLogger.Object
            );

            this.random = new Random();

            this.accountEntityBuilder = new AccountEntityBuilder();
            this.accountModelBuilder =  new AccountModelBuilder();
        }
    }
}
