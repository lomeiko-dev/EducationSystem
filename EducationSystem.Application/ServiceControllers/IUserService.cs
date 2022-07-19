
using Microsoft.AspNetCore.Http;

namespace EducationSystem.Application.ServiceControllers
{
    public interface IUserService<TResponse, TUser, TId>
    {
        public Task<TResponse> AddToRolesAsync(TId id, IEnumerable<string> roles);
        public Task<TResponse> DeleteToRolesAsync(TId id, IEnumerable<string> roles);
        public Task<TResponse> GetUserAsync(TId id);
        public Task<TResponse> GetUsersPageAsync(int skip, int take);
        public Task<TResponse> UpdateUserAsync(TId id, TUser entity);
        public Task<TResponse> SetBlockUser(TId id, string time);
    }
}
