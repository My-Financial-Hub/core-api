using System.Linq;
using FinancialHub.Core.Infra.Data.Contexts;
using FinancialHub.Common.Entities;
using FinancialHub.Common.Interfaces.Repositories;

namespace FinancialHub.Core.Infra.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly FinancialHubContext context;
        public BaseRepository(FinancialHubContext context)
        {
            this.context = context;
        }

        public virtual async Task<T> CreateAsync(T obj)
        {
            obj.Id = null;
            obj.CreationTime = DateTimeOffset.Now;
            obj.UpdateTime = DateTimeOffset.Now;

            var res = await context.Set<T>().AddAsync(obj);
            await context.SaveChangesAsync();
            return res.Entity;
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = await this.GetByIdAsync(id);

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

        public virtual async Task<T> UpdateAsync(T obj)
        {
            obj.UpdateTime = DateTimeOffset.Now;

            var res = context.Set<T>().Update(obj);
            this.context.Entry(res.Entity).Property(x => x.CreationTime).IsModified = false;

            await context.SaveChangesAsync();

            return res.Entity;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
        public virtual async Task<ICollection<T>> GetAsync(Func<T, bool> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }
#pragma warning restore CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
