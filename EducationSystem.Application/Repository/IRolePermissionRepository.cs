
namespace EducationSystem.Application.Repository
{
    public interface IRolePermissionRepository<TReturn, TPermission, TId>
    {
        public Task<TReturn> CreateAsync(TPermission entity);
        public Task<IEnumerable<TPermission>> GetPermissionsAsync(TId id);
        public Task<IEnumerable<TPermission>> GetPageAsync(int skip, int take);
        public Task<TReturn> DeleteAsync(TId id);
    }
}
