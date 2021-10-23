using AutoMapper;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
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

        public async Task<ICollection<AccountModel>> GetAccountsByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();
            return mapper.Map<ICollection<AccountModel>>(entities);
        }
    }
}
