using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class CategoriesRepository : BaseRepository<CategoryEntity>, ICategoriesRepository
    {
        public CategoriesRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
