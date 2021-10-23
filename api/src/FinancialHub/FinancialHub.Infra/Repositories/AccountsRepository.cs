using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;
using System;
namespace FinancialHub.Infra.Repositories
{
    public class AccountsRepository : BaseRepository<AccountEntity>, IAccountsRepository
    {
        public AccountsRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
