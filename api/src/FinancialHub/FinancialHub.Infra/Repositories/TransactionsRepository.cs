using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;

namespace FinancialHub.Infra.Repositories
{
    public class TransactionsRepository : BaseRepository<TransactionEntity>, ITransactionsRepository
    {
        public TransactionsRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
