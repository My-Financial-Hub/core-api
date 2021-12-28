using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Filters;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Queries;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;

namespace FinancialHub.Services.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IMapperWrapper mapper;
        private readonly ITransactionsRepository repository;

        public TransactionsService(IMapperWrapper mapper, ITransactionsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ServiceResult<TransactionModel>> CreateAsync(TransactionModel category)
        {
            var entity = mapper.Map<TransactionEntity>(category);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<TransactionModel>>> GetAllByUserAsync(string userId, TransactionFilter filter)
        {
            var query = mapper.Map<TransactionQuery>(filter);

            var entities = await this.repository.GetAsync(query.Query());

            var models = mapper.Map<ICollection<TransactionModel>>(entities);

            return models.ToArray();
        }

        public async Task<ServiceResult<TransactionModel>> UpdateAsync(Guid id, TransactionModel transaction)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new NotFoundServiceError($"Not found transaction with id {id}");
            }

            entity = this.mapper.Map<TransactionEntity>(transaction);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }
    }
}
