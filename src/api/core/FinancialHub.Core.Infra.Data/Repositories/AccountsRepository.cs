using FinancialHub.Core.Infra.Data.Contexts;

namespace FinancialHub.Core.Infra.Data.Repositories
{
    internal class AccountsRepository : BaseRepository<AccountEntity>, IAccountsRepository
    {
        public AccountsRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
