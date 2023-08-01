using AutoMapper;
using FinancialHub.Core.Domain.Interfaces.Mappers;
using FinancialHub.Core.Domain.Interfaces.Repositories;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Services.Mappers;
using FinancialHub.Services.Services;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryEntityBuilder categoryBuilder; 
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<ICategoriesRepository> repository;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new FinancialHubAutoMapperProfile());
                }
            ).CreateMapper();

            this.mapperWrapper = new Mock<IMapperWrapper>();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository = new Mock<ICategoriesRepository>();
            this.service = new CategoriesService(mapperWrapper.Object,repository.Object);

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
