using EducationSystem.Application.Repository;
using EducationSystem.Core.Entity.Role;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.JWT;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;

namespace EducationSystem.Helper.Extensions
{
    public static class ControllerExtensions
    {

        #region wraps

        public static async Task<IActionResult> Wrap(this ControllerBase controller, Func<Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke();
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, controller.ModelState.Select(item => item.Value.Errors.Select(error => error.ErrorMessage)));
        }
        public static async Task<IActionResult> Wrap<TItem>(this ControllerBase controller, TItem item, Func<TItem, Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, controller.ModelState.Select(item => item.Value.Errors.Select(error => error.ErrorMessage)));
        }
        public static async Task<IActionResult> Wrap<TItem, TItem2>(this ControllerBase controller, TItem item, TItem2 item2, Func<TItem, TItem2, Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item, item2);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, controller.ModelState.Select(item => item.Value.Errors.Select(error => error.ErrorMessage)));
        }

        public static async Task<IActionResult> Wrap<TItem, TItem2, TItem3>(this ControllerBase controller, TItem item, TItem2 item2, TItem3 item3, Func<TItem, TItem2, TItem3, Task<BaseResponse>> func)
        {
            if (controller.ModelState.IsValid)
            {
                var result = await func.Invoke(item, item2, item3);
                return controller.StatusCode(result.StatusCode, result.Value);
            }
            return controller.StatusCode(400, controller.ModelState.Select(item => item.Value.Errors.Select(error => error.ErrorMessage)));
        }

        #endregion


        public static void CorrectHttpContextByBearrer(this ControllerBase controller)
        {
            var jwtManager = controller.HttpContext.RequestServices.GetService<JwtManager>();

            var token = controller.HttpContext.Request.Headers["Authorization"].ToString().Substring(7);
            controller.HttpContext.User = jwtManager.GetPrincipal(token);
        }

        public static async Task<int> RolePermissionMethodAsync(this ControllerBase controller, string tag)
        {
            var roleManager = controller.HttpContext.RequestServices.GetService<RoleManager<Role>>();
            var rolePermissionRepository = controller.HttpContext.RequestServices.GetService<IRolePermissionRepository<bool, RolePermission, string>>();

            // get list roles string by context
            var issuerRoles = controller.HttpContext.User.Identities.ToList()[0].Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value.Split(" ").ToList();
            if (issuerRoles.Count == 0)
                return 403;

            var roles = await roleManager.FindByListNameAsync(issuerRoles);
            var permissions = await rolePermissionRepository.GetPermissionsAsync(roles.MinBy(x => x.Level).Id.ToString());

            if (permissions.FirstOrDefault(x => x.PermissionTag == tag) == null)
                return 403;

            return 200;
        }

        public static async Task<bool> CheckBlockAsync(this ControllerBase controller)
        {
            var userManager = controller.HttpContext.RequestServices.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(controller.HttpContext.User.Identities.ToList()[0].Claims.FirstOrDefault(x => x.Type == "Id").Value);
            if(userManager.Locko)
        }
    }
}
