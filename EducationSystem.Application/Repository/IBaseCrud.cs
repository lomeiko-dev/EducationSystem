
namespace EducationSystem.Application.Repository
{
    public interface IBaseCrud<TReturn, TEntity, TId>
    {
        public Task<TReturn> CreateAsync(TEntity entity);
        public Task<TEntity> GetAsync(TId id);
        public Task<TReturn> UpdateAsync(TEntity entity, TId id);
        public Task DeleteAsync(TEntity entity);
    }
}
