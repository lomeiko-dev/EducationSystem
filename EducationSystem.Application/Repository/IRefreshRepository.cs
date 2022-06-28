namespace EducationSystem.Application.Repository
{
    public interface IRefreshRepository<TResponse, TEntity, TId>
    {
        public Task<TResponse> CreateTokenAsync(TEntity entity);
        public Task<TEntity> GetTokenByUserIdAsync(TId id_user);
        public Task<TEntity> GetTokenByRefreshToken(string refresh);
        public Task DeleteTokenAsync(TEntity entity);
    }
}
