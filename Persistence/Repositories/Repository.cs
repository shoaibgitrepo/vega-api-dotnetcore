using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega_api_dotnetcore.Core.Repositories;

namespace vega_api_dotnetcore.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> GetEntityAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }


        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
        }


        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
        }
    }
}