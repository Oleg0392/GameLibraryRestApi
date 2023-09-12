using System.Linq.Expressions;

namespace GameLibraryRestApi.Repositories.Interfaces
{
    public interface IEFCoreRepository<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity);

        Task<IList<TEntity>> InsertRangeAsync(IList<TEntity> entities, bool saveChanges = true);

        Task<List<TEntity>> GetAllAsync();

        Task<List<TEntity>> FindAllByWhereAsync(Expression<Func<TEntity, bool>> match);

        Task<TEntity> FindFirstWhereAsync(Expression<Func<TEntity, bool>> match);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

    }
}
