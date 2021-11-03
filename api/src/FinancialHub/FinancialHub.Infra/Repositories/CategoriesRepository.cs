using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;

namespace FinancialHub.Infra.Repositories
{
    public class CategoriesRepository : BaseRepository<CategoryEntity>, ICategoriesRepository
    {
        public CategoriesRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
