using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        protected Random random;
        protected AccountModelBuilder accountModelBuilder; 
        
        private IAccountsService service;

        private Mock<IAccountsProvider> provider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<IAccountsProvider>();
            this.service = new AccountsService(provider.Object);

            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();
        }
    }
}
