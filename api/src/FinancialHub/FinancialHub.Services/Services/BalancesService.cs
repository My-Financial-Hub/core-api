using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Results.Errors;

namespace FinancialHub.Services.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IMapperWrapper mapper;
        private readonly IBalancesRepository repository;

        public BalancesService(IMapperWrapper mapper, IBalancesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ServiceResult<BalanceModel>> CreateAsync(BalanceModel balance)
        {
            var entity = this.mapper.Map<BalanceEntity>(balance);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<BalanceModel>(entity);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            var count = await this.repository.DeleteAsync(id);

            return new ServiceResult<int>(count);
        }

        public async Task<ServiceResult<ICollection<BalanceModel>>> GetAllByAccountAsync(Guid accountId)
        {
            var entities = await this.repository.GetAsync(x => x.AccountId == accountId);

            var list = this.mapper.Map<ICollection<BalanceModel>>(entities);

            return list.ToArray();
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAsync(Guid id, BalanceModel balance)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new NotFoundError($"Not found balance with id {id}");
            }

            entity = this.mapper.Map<BalanceEntity>(balance);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<BalanceModel>(entity);
        }
    }
}
