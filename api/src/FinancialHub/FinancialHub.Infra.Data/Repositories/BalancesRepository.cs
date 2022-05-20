using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class BalancesRepository : BaseRepository<BalanceEntity>, IBalancesRepository
    {
        public BalancesRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
