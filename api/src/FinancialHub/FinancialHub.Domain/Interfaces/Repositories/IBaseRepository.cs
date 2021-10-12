using System;
using System.Threading.Tasks;
using FinancialHub.Domain.Model;
using System.Collections.Generic;

namespace FinancialHub.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T>
        where T : BaseModel
    {
        Task<T> CreateAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task<int> DeleteAsync(string id);
        Task<ICollection<T>> GetAsync();
        Task<ICollection<T>> GetAsync(Func<T, bool> predicate);
    }
}
