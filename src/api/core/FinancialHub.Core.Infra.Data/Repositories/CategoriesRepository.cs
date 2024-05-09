using FinancialHub.Core.Infra.Data.Contexts;

namespace FinancialHub.Core.Infra.Data.Repositories
{
    internal class CategoriesRepository : BaseRepository<CategoryEntity>, ICategoriesRepository
    {
        public CategoriesRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
