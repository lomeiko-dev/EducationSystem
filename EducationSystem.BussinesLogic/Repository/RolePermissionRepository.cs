
using EducationSystem.Application.Repository;
using EducationSystem.Core.Entity.Role;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationSystem.BussinesLogic.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository<bool, RolePermission, string>
    {
        private readonly ApplicationContext applicationContext;

        public RolePermissionRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(RolePermission entity)
        {
            if (entity == null)
                return false;

            await applicationContext.RolePermissions.AddAsync(entity);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var permission = await applicationContext.RolePermissions.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (permission == null)
                return false;

            applicationContext.Remove(permission);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<RolePermission>> GetPageAsync(int skip, int take)
        {
            var permissions = await applicationContext.RolePermissions.Skip(skip).Take(take).ToListAsync();
            return permissions;
        }

        public async Task<IEnumerable<RolePermission>> GetPermissionsAsync(string id)
        {
            var permissions = await applicationContext.RolePermissions.Where(x => x.Id_Role == id).ToListAsync();
            return permissions;
        }
    }
}
