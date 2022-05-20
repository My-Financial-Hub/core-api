using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class BalanceRepository : BaseRepository<BalanceEntity>, IBalanceRepository
    {
        public BalanceRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
