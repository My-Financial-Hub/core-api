using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Queries;

namespace FinancialHub.Core.Infra.Providers
{
    public class TransactionsProvider : ITransactionsProvider
    {
        private readonly IMapper mapper;
        private readonly ITransactionsRepository repository;

        public TransactionsProvider(IMapper mapper, ITransactionsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<TransactionModel> CreateAsync(TransactionModel transaction)
        {
            var entity = mapper.Map<TransactionEntity>(transaction);
            entity = await this.repository.CreateAsync(entity);
            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<TransactionModel>> GetAllAsync(TransactionFilter filter)
        {
            var query = mapper.Map<TransactionQuery>(filter);

            var entities = await this.repository.GetAsync(query.Query());

            return mapper.Map<ICollection<TransactionModel>>(entities);
        }

        public async Task<TransactionModel?> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.GetByIdAsync(id);
            if(entity == null)
                return null;
        
            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<TransactionModel> UpdateAsync(Guid id, TransactionModel transaction)
        {
            var entity = mapper.Map<TransactionEntity>(transaction);
            entity.Id = id;

            var updated = await this.repository.UpdateAsync(entity);
            return mapper.Map<TransactionModel>(updated);
        }
    }
}
