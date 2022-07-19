
using EducationSystem.Core.Entity.Role;
using Microsoft.AspNetCore.Identity;

namespace EducationSystem.Helper.Extensions
{
    public static class RoleManagerExtensions
    {
        public static async Task<List<Role>> FindByListNameAsync(this RoleManager<Role> roleManager, List<string> rolenames)
        {
            var result = new List<Role>();
            foreach (var item in rolenames)
                result.Add(await roleManager.FindByNameAsync(item));
            return result;
        }
    }
}
