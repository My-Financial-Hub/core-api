using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;

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
        private Mock<IErrorMessageProvider> errorMessageProvider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ITransactionsProvider>();
            this.balancesProvider = new Mock<IBalancesProvider>();
            this.categoriesProvider = new Mock<ICategoriesProvider>();
            this.errorMessageProvider = new Mock<IErrorMessageProvider>();
            this.service = new TransactionsService(
                provider.Object,
                balancesProvider.Object,
                categoriesProvider.Object,
                errorMessageProvider.Object
            );

            this.random = new Random();

            this.transactionModelBuilder = new TransactionModelBuilder();
        }
    }
}
