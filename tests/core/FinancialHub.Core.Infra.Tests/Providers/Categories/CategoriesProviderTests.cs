using FinancialHub.Core.Infra.Mappers;
using FinancialHub.Core.Infra.Providers;

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
            this.provider = new CategoriesProvider(this.mapper, this.repository.Object);

            this.random = new Random();

            this.categoryBuilder = new CategoryEntityBuilder();
            this.categoryModelBuilder = new CategoryModelBuilder();
        }
    }
}
