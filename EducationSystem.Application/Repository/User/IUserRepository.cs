
namespace EducationSystem.Application.Repository.User
{
    public interface IUserRepository<TReturn, TEntity, TId> : IBaseCrud<TReturn, TEntity, TId>
    {
        public Task<IEnumerable<TEntity>> GetPagesAsync(int skip, int take);
        public Task<TEntity> FindUserByEmail(string email);
        public Task<TEntity> FindUserByNumber(string number);

        
    }
}
