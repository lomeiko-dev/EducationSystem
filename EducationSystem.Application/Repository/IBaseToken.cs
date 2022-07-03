
namespace EducationSystem.Application.Repository
{
    public interface IBaseToken<TReturn, TEntity>
    {
        public Task<TReturn> CreateTokenAsync(TEntity entity);
        public Task<TEntity> GetTokenAsync(string token);
        public Task DeleteTokenAsync(TEntity entity);
    }
}
