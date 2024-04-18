using FinancialHub.Core.Domain.Interfaces.Caching;
using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class CategoriesProviderTests
    {
        protected Random random;
        protected CategoryEntityBuilder categoryBuilder;
        protected CategoryModelBuilder categoryModelBuilder;

        private ICategoriesProvider provider;

        private IMapper mapper;
        private Mock<ICategoriesRepository> repository;
        private Mock<ICategoriesCache> cache;
        private Mock<ILogger<CategoriesProvider>> logger;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAutoMapperProfile());
            }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository = new Mock<ICategoriesRepository>();
            this.cache      = new Mock<ICategoriesCache>();
            this.logger     = new Mock<ILogger<CategoriesProvider>>();

            this.provider = new CategoriesProvider(
                this.repository.Object, this.cache.Object,
                this.mapper,
                this.logger.Object
            );

            this.random = new Random();

            this.categoryBuilder = new CategoryEntityBuilder();
            this.categoryModelBuilder = new CategoryModelBuilder();
        }
    }
}
