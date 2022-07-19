using EducationSystem.Application.Repository;
using EducationSystem.Application.Validate;
using EducationSystem.Core.Entity.Role;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Extensions;
using EducationSystem.Helper.Request;
using Microsoft.AspNetCore.Identity;

namespace EducationSystem.BussinesLogic.Validate
{
    public class ValidatePermissionChangeUser : IValidate<User, RequestValidateChangeUser>
    {
        private readonly RoleManager<Role> roleManager;
        private readonly IRolePermissionRepository<bool, RolePermission, string> rolePermissionRepository;
        public ValidatePermissionChangeUser(RoleManager<Role> roleManager,
                                            IRolePermissionRepository<bool, RolePermission, string> rolePermissionRepository)
        {
            this.roleManager = roleManager;
            this.rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<User> ValidateAsync(RequestValidateChangeUser obj)
        {
            var user = new User();

            var permissions = await rolePermissionRepository.GetPermissionsAsync(obj.Role.Id.ToString());
            if(permissions.FirstOrDefault(x => x.PermissionTag == "updatingSchoolUser") != null) 
            {
                user.Id_Class = obj.NewUser.Id_Class;
                user.Id_School = obj.NewUser.Id_School;
            }
            if (permissions.FirstOrDefault(x => x.PermissionTag == "updatingUserData") != null)
            {
                user.FullName = obj.NewUser.FullName;
                user.UserName = obj.NewUser.UserName;
                user.HomeAddress = obj.NewUser.HomeAddress;
                user.PhoneNumber = obj.NewUser.PhoneNumber;
            }

            return user;
        }
    }
}
