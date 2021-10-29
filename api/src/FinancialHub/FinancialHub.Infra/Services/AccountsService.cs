using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.Infra.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapper mapper;
        private readonly IAccountsRepository repository;

        public AccountsService(IMapper mapper,IAccountsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<AccountModel> CreateAsync(AccountModel account)
        {
            var entity = mapper.Map<AccountEntity>(account);
            entity.CreationTime = DateTimeOffset.Now;
            entity.UpdateTime   = DateTimeOffset.Now;

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }

        public async Task<int> DeleteAsync(string id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<AccountModel>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<AccountModel>>(entities);
        }

        public async Task<AccountModel> UpdateAsync(string id, AccountModel account)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if(entity == null)
            {
                throw new NullReferenceException($"Not found account with id {id}");
            }
            entity.Id = new Guid(id);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }
    }
}
