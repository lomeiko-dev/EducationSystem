
namespace EducationSystem.Application.Repository.User
{
    public interface IConfirmTokenRepository<TReturn,TEntity, TId>
    {
        public Task<TReturn> CreateAsync(TEntity entity);
        public Task<TEntity> GetAsync(TId id_user);
        public Task DeleteAsync(TEntity entity);
    }
}
