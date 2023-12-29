using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private Mock<ICategoriesProvider> provider;
        private Mock<IErrorMessageProvider> errorMessageProvider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ICategoriesProvider>();
            this.errorMessageProvider = new Mock<IErrorMessageProvider>();
            this.service = new CategoriesService(provider.Object, errorMessageProvider.Object);

            this.random = new Random();

            this.categoryModelBuilder = new CategoryModelBuilder();
        }
    }
}
