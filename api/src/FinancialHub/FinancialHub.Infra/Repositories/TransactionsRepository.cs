using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;
using System;
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
            #warning This is not a good practice, remove this later
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Category = null;
            obj.Account = null;
            return await base.CreateAsync(obj);
        }

        public override async Task<TransactionEntity> UpdateAsync(TransactionEntity obj)
        {
            obj.Category = null;
            obj.Account = null; 
            return await base.UpdateAsync(obj);
        }
    }
}
