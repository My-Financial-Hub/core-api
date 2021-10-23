using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FinancialHub.Domain.Entities;

namespace FinancialHub.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Base repository with basic CRUD methods
    /// </summary>
    /// <typeparam name="T">Any Entity that inherits <see cref="BaseEntity"/> </typeparam>
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Adds an entity to the database 
        /// </summary>
        /// <param name="obj">Entity to be added</param>
        /// <returns></returns>
        Task<T> CreateAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task<int> DeleteAsync(string id);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAsync(Func<T, bool> predicate);
        Task<T> GetByIdAsync(string id);
    }
}
