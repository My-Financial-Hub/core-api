using FinancialHub.Common.Interfaces.Repositories;
using FinancialHub.Core.Domain.Entities;

namespace FinancialHub.Core.Domain.Interfaces.Repositories
{
    public interface IAccountsRepository : IBaseRepository<AccountEntity>
    {
        //Task<ICollection<<AccountEntity>> GetByUserId(string userId);
    }
}
