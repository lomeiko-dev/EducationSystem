
using System.Linq.Expressions;

namespace EducationSystem.Application.Repository
{
    public interface IBaseCrud<TReturn, TEntity, TId>
    {
        public Task<TReturn> CreateAsync(TEntity entity);
        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> func);
        public Task<IEnumerable<TEntity>> GetPageAsync(int skip, int take);
        public Task<TReturn> UpdateAsync(TId id, TEntity entity);
        public Task<TReturn> DeleteAsync(TId id);
    }
}
