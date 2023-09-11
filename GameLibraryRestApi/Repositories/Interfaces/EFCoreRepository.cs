using Microsoft.EntityFrameworkCore;
using GameLibraryRestApi.Data.DbContexts;
using System.Linq.Expressions;

namespace GameLibraryRestApi.Repositories.Interfaces
{
    public abstract class EFCoreRepository<TEntity> : IEFCoreRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        //private readonly DbSet<TEntity> dbSet;

        protected EFCoreRepository(GameLibraryContext context)
        {
            _context = context;
            //dbSet = context.Set<TEntity>();
        }
        
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IList<TEntity>> InsertRangeAsync(IList<TEntity> entities, bool saveChanges = true)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
            return entities;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> FindAllByWhereAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _context.Set<TEntity>().Where(match).ToListAsync();
        }

        public virtual async Task<TEntity> FindFirstWhereAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(match);
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
