using FinancialHub.Infra.Data.Contexts;

namespace FinancialHub.Infra.Data.Repositories
{
    public class AccountsRepository : BaseRepository<AccountEntity>, IAccountsRepository
    {
        public AccountsRepository(FinancialHubContext context) : base(context)
        {
        }
    }
}
