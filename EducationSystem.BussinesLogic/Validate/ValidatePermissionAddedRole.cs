using EducationSystem.Application.Validate;
using EducationSystem.Core.Role;
using EducationSystem.Helper.Extensions;
using EducationSystem.Helper.Request;
using Microsoft.AspNetCore.Identity;

namespace EducationSystem.BussinesLogic.Validate
{
    public class ValidatePermissionAddedRole : IValidate<IEnumerable<string>, RequestValidateAddedRole>
    {
        private readonly RoleManager<Role> roleManager;

        public ValidatePermissionAddedRole(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<string>> ValidateAsync(RequestValidateAddedRole obj)
        {
            return await Task.Run(async () =>
            {
                var issuerRoles = await roleManager.FindByListNameAsync(obj.IssuerRoles);
                var highRole = issuerRoles.MinBy(x => x.Level);

                var response = new List<string>();
                foreach (var item in obj.Roles)
                {
                    var role = await roleManager.FindByNameAsync(item);
                    if(role != null)
                        if (highRole.Level < role.Level)
                            response.Add(role.Name);
                }
                   
                return response;
            });
        }
    }
}
