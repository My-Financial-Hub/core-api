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
    public class TransactionsService : ITransactionsService
    {
        private readonly IMapper mapper;
        private readonly ITransactionsRepository repository;

        public TransactionsService(IMapper mapper, ITransactionsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<TransactionModel> CreateAsync(TransactionModel category)
        {
            var entity = mapper.Map<TransactionEntity>(category);
            entity.CreationTime = DateTimeOffset.Now;
            entity.UpdateTime = DateTimeOffset.Now;

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<int> DeleteAsync(string id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ICollection<TransactionModel>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<TransactionModel>>(entities);
        }

        public async Task<TransactionModel> UpdateAsync(string id, TransactionModel category)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NullReferenceException($"Not found category with id {id}");
            }
            entity.Id = new Guid(id);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<TransactionModel>(entity);
        }
    }
}
