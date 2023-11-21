using FinancialHub.Core.Application.Services;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        protected Random random;
        protected TransactionModelBuilder transactionModelBuilder; 
        
        private ITransactionsService service;

        private Mock<ITransactionsProvider> provider;
        private Mock<IBalancesProvider> balancesProvider;
        private Mock<ICategoriesProvider> categoriesProvider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ITransactionsProvider>();
            this.balancesProvider = new Mock<IBalancesProvider>();
            this.categoriesProvider = new Mock<ICategoriesProvider>();
            this.service = new TransactionsService(
                provider.Object,
                balancesProvider.Object,
                categoriesProvider.Object
            );

            this.random = new Random();

            this.transactionModelBuilder = new TransactionModelBuilder();
        }
    }
}
