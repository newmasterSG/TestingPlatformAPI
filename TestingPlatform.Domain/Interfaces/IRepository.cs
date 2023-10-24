using System.Linq.Expressions;

namespace TestingPlatform.Domain.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity> GetEntityAsync(int id);

        void Update(TEntity entity);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> TakeAsync(int skipElements, int takeElements, (Expression<Func<TEntity, object>> expression, bool ascending) sortOrder);
    }
}
