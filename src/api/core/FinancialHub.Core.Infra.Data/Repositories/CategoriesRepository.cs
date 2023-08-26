using FinancialHub.Core.Infra.Data.Contexts;

namespace FinancialHub.Core.Infra.Data.Repositories
{
    public class CategoriesRepository : BaseRepository<CategoryEntity>, ICategoriesRepository
    {
        public CategoriesRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
