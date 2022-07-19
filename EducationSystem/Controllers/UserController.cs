using EducationSystem.Application.ServiceControllers;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Extensions;
using EducationSystem.Helper.Options.OptionsConst;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using EducationSystem.Web.Api.Contracts.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Web.Api.Controllers
{
    [Route(Routes.ControllerUser)]
    public class UserController : Controller
    {
        private readonly IUserService<BaseResponse, User, string> userService;

        public UserController(IUserService<BaseResponse, User, string> userService)
        {
            this.userService = userService;
        }

        [HttpPost(Routes.AddToRole)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddToRolesAsync([Required] string id, [FromBody]RequestRoles request)
        {
            this.CorrectHttpContextByBearrer();
            var result = await this.RolePermissionMethodAsync(tag:"AddToRoles");
            if (result == 403)
                return new StatusCodeResult(result);
            return await this.Wrap(id, request.Roles, userService.AddToRolesAsync);
        }

        [HttpPost(Routes.RemoveToRole)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveToRolesAsync([Required] string id, [FromBody] RequestRoles request)
        {
            this.CorrectHttpContextByBearrer();
            var result = await this.RolePermissionMethodAsync(tag: "RemoveToRoles");
            if (result == 403)
                return new StatusCodeResult(result);
            return await this.Wrap(id, request.Roles, userService.DeleteToRolesAsync);
        }

        [HttpGet(Routes.GetPage)]
        public async Task<IActionResult> GetUsersPageAsync([Required] int skip, [Required] int take)
        {
            return await this.Wrap(skip, take, userService.GetUsersPageAsync);
        }

        [HttpPut(Routes.Update)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUserAsync([Required] string id, [FromBody]User user)
        {
            this.CorrectHttpContextByBearrer();
            var result = await this.RolePermissionMethodAsync(tag: "UpdateUser");
            if (result == 403)
                return new StatusCodeResult(result);
            return await this.Wrap(id, user, userService.UpdateUserAsync);
        }
    }
}
