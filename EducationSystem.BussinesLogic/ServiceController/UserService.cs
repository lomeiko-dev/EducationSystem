using EducationSystem.Application.ServiceControllers;
using EducationSystem.Application.Validate;
using EducationSystem.Core.Entity.Role;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EducationSystem.BussinesLogic.ServiceController
{
    public class UserService : IUserService<BaseResponse, User, string>
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly IValidate<IEnumerable<string>, RequestValidateAddedRole> validatePermissionAddedRole;
        private readonly IValidate<User, RequestValidateChangeUser> validatePermissionChangeUser;
        private readonly HttpContext context;

        public UserService(UserManager<User> userManager,
                           RoleManager<Role> roleManager,
                           IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                           IValidate<IEnumerable<string>, RequestValidateAddedRole> validatePermissionRole,
                           IActionContextAccessor actionContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.validatePermissionAddedRole = validatePermissionRole;
            context = actionContextAccessor.ActionContext.HttpContext;
        }

        public async Task<BaseResponse> AddToRolesAsync(string id, IEnumerable<string> roles) =>
              await RoleMethodsAsync(id, roles, userManager.AddToRolesAsync);

        public async Task<BaseResponse> DeleteToRolesAsync(string id, IEnumerable<string> roles) =>
              await RoleMethodsAsync(id, roles, userManager.RemoveFromRolesAsync);

        private async Task<BaseResponse> RoleMethodsAsync(string id,
                                                          IEnumerable<string> roles,
                                                          Func<User, IEnumerable<string>, Task<IdentityResult>> func)
        {
            // find user for give role
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            // get list roles string by context
            var issuerRoles = context.User.Identities.ToList()[0].Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value.Split(" ").ToList();
            if (issuerRoles.Count == 0)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user roles"), 404);

            // validate role, get allowed roles
            var allowedRoles = await validatePermissionAddedRole.ValidateAsync(new RequestValidateAddedRole
            {
                IssuerRoles = issuerRoles,
                Roles = roles.ToList()
            });
            if (allowedRoles.ToList().Count == 0)
                return new BaseResponse(optionsBaseAnswer.Forbidden, 403);

            // added roles
            var result = await func.Invoke(user, allowedRoles);
            if (!result.Succeeded)
                return new BaseResponse(result.Errors.Select(x => x.Description).ToList(), 400);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> GetUsersPageAsync(int skip, int take)
        {
            var users = await userManager.Users.Skip(skip).Take(take).ToListAsync();
            if (users.Count == 0)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "users"), 404);

            return new BaseResponse(users, 200);
        }

        public async Task<BaseResponse> UpdateUserAsync(string id, User entity)
        {
            // find user for update
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            // get list roles string by context
            var roles = context.User.Identities.ToList()[0].Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value.Split(" ").ToList();
            if (roles.Count == 0)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user roles"), 404);

            // validate and get permision property user
            var permisionUser = await validatePermissionChangeUser.ValidateAsync(
                    new RequestValidateChangeUser { Role = await roleManager.FindByNameAsync(roles[0]), NewUser = entity });
            if (permisionUser == null)
                return new BaseResponse(optionsBaseAnswer.Forbidden, 403);

            // update
            user.FullName = permisionUser.FullName ?? user.FullName;
            user.UserName = permisionUser.UserName ?? user.UserName;
            user.HomeAddress = permisionUser.HomeAddress ?? user.HomeAddress;
            user.Id_Class = permisionUser.Id_Class ?? user.Id_Class;
            user.Id_School = permisionUser.Id_School ?? user.Id_School;
            user.PhoneNumber = permisionUser.PhoneNumber ?? user.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if(!result.Succeeded)
                return new BaseResponse(result.Errors.Select(x => x.Description).ToList(), 400);

            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public Task<BaseResponse> GetUserAsync(string id)
        {

        }

        public Task<BaseResponse> SetBlockUser(string id, string time)
        {

        }
    }
}
