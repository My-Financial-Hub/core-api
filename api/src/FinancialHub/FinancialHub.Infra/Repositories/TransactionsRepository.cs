using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Repositories
{
    public class TransactionsRepository : BaseRepository<TransactionEntity>, ITransactionsRepository
    {
        public TransactionsRepository(FinancialHubContext context) : base(context)
        {
        }

        public override async Task<TransactionEntity> CreateAsync(TransactionEntity obj)
        {
            obj.Id = null;
            obj.Category = null;
            obj.Account = null;

            var res = await context.Transactions.AddAsync(obj);
            await context.SaveChangesAsync();
            return res.Entity;
        }
    }
}
