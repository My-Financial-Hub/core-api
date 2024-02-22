using AutoMapper;
using FinancialHub.Core.Application.Mappers;
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
        private IMapper mapper;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new CategoryMapper());
                }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<ICategoriesProvider>();
            this.errorMessageProvider = new Mock<IErrorMessageProvider>();
            this.MockMapper();
            this.service = new CategoriesService(
                provider.Object, 
                errorMessageProvider.Object, 
                this.mapper
            );

            this.random = new Random();

            this.categoryModelBuilder = new CategoryModelBuilder();
            this.AddCreateCategoryBuilder();
            this.AddUpdateCategoryBuilder();
        }
    }
}
