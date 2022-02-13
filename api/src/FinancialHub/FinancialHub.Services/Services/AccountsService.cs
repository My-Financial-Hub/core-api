using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;

namespace FinancialHub.Services.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapperWrapper mapper;
        private readonly IAccountsRepository repository;

        public AccountsService(IMapperWrapper mapper,IAccountsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var entity = mapper.Map<AccountEntity>(account);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<AccountModel>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<AccountModel>>(entities);
        }

        public async Task<AccountModel> UpdateAsync(Guid id, AccountModel account)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if(entity == null)
            {
                throw new NullReferenceException($"Not found account with id {id}");
            }
            entity.Id = id;

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }
    }
}
