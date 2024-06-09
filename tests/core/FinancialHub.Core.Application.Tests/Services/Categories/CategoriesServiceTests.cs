using AutoMapper;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private Mock<ICategoriesProvider> provider;
        private Mock<ICategoriesValidator> validator;
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
            this.validator = new Mock<ICategoriesValidator>();
            this.MockMapper();
            this.service = new CategoriesService(
                this.provider.Object,
                this.validator.Object, 
                this.mapper, 
                new NullLoggerFactory().CreateLogger<CategoriesService>()
            );

            this.random = new Random();

            this.categoryModelBuilder = new CategoryModelBuilder();
            this.AddCreateCategoryBuilder();
            this.AddUpdateCategoryBuilder();
        }
    }
}
