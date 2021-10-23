using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Infra.Contexts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinancialHub.Infra.Repositories
{
    public abstract class BaseRepository<T> :
        IBaseRepository<T>
        where T : BaseEntity
    {
        private readonly FinancialHubContext context;
        public BaseRepository(FinancialHubContext context)
        {
            this.context = context;
        }

        public async Task<T> CreateAsync(T obj)
        {
            var res = await context.AddAsync(obj);
            await context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var entity = context.Set<T>().FirstOrDefault(x => x.Id.ToString() == id);

            if(entity != null)
            {
                context.Set<T>().Remove(entity);
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<T> UpdateAsync(T obj)
        {
            obj.UpdateTime = DateTimeOffset.Now;
            var res = context.Set<T>().Update(obj);
            await context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<ICollection<T>> GetAsync(Func<T, bool> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id.ToString() == id);
        }
    }
}
