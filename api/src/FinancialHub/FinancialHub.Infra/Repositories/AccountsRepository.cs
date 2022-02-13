using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Data.Contexts;
using System.ComponentModel;

namespace FinancialHub.Infra.Data.Repositories
{
    [Category("Repositories")]
    public class AccountsRepository : BaseRepository<AccountEntity>, IAccountsRepository
    {
        public AccountsRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
