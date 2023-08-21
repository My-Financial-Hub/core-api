using FinancialHub.Core.Infra.Data.Tests.Repositories.Base;
using FinancialHub.Core.Infra.Data.Repositories;

namespace FinancialHub.Core.Infra.Data.Tests.Repositories
{
    public class CategoriesRepositoryTests : BaseRepositoryTests<CategoryEntity>
    {
        [SetUp]
        protected override void Setup()
        {
            base.Setup();
            this.repository = new CategoriesRepository(this.context);
            this.builder = new CategoryEntityBuilder();
        }
    }
}
