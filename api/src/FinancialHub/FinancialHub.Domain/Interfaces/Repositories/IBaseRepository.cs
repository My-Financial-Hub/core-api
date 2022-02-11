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
        Task<T> CreateAsync(T obj);
        /// <summary>
        /// Updates a created entity on the database
        /// </summary>
        /// <param name="obj">Entity on the database</param>
        Task<T> UpdateAsync(T obj);
        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        /// <param name="id">Id of the entity to be removed</param>
        Task<int> DeleteAsync(Guid id);
        /// <summary>
        /// Get All entities from the database
        /// </summary>
        Task<ICollection<T>> GetAllAsync();
        /// <summary>
        /// Get All entities from the database based on a filter
        /// </summary>
        Task<ICollection<T>> GetAsync(Func<T, bool> predicate);
        /// <summary>
        /// Gets an entity by id
        /// </summary>
        /// <param name="id">Id of the choosen entity</param>
        Task<T> GetByIdAsync(Guid id);
    }
}
