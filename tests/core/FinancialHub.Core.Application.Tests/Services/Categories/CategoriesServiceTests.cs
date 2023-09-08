using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Providers;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryEntityBuilder categoryBuilder; 
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private Mock<ICategoriesProvider> provider;

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ICategoriesProvider>();
            this.service = new CategoriesService(provider.Object);

            this.random = new Random();

            this.categoryBuilder = new CategoryEntityBuilder();
            this.categoryModelBuilder = new CategoryModelBuilder();
        }

        private ICollection<CategoryEntity> CreateCategories()
        {
            return this.categoryBuilder.Generate(random.Next(10, 100));
        }
    }
}
