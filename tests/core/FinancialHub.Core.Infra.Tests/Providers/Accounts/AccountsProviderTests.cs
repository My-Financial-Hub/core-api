using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;

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

            this.repository = new Mock<IAccountsRepository>();
            this.provider = new AccountsProvider(this.mapper, this.repository.Object);

            this.random = new Random();

            this.accountEntityBuilder = new AccountEntityBuilder();
            this.accountModelBuilder =  new AccountModelBuilder();
        }
    }
}
