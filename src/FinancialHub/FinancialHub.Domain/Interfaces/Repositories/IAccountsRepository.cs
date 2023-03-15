using FinancialHub.Domain.Entities;

namespace FinancialHub.Domain.Interfaces.Repositories
{
    public interface IAccountsRepository : IBaseRepository<AccountEntity>
    {
        //Task<ICollection<<AccountEntity>> GetByUserId(string userId);
    }
}
