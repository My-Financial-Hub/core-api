using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class TransactionsRepository : BaseRepository<TransactionEntity>, ITransactionsRepository
    {
        public TransactionsRepository(FinancialHubContext context) : base(context)
        {
        }

        public override async Task<TransactionEntity> CreateAsync(TransactionEntity obj)
        {
            #warning This is not a good practice, remove this later
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Category = null;
            obj.Balance = null;
            return await base.CreateAsync(obj);
        }

        public override async Task<TransactionEntity> UpdateAsync(TransactionEntity obj)
        {
            obj.Category = null;
            obj.Balance = null; 
            return await base.UpdateAsync(obj);
        }
    }
}
