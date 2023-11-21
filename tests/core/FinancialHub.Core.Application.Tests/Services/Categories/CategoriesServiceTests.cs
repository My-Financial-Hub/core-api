using FinancialHub.Core.Application.Services;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private Mock<ICategoriesProvider> provider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ICategoriesProvider>();
            this.service = new CategoriesService(provider.Object);

            this.random = new Random();

            this.categoryModelBuilder = new CategoryModelBuilder();
        }
    }
}
