using EducationSystem.Helper.Response;
using EducationSystem.Helper.Extensions;
using Microsoft.AspNetCore.Mvc;
using EducationSystem.Web.Api.Contracts.v1;
using EducationSystem.Helper.Request;
using EducationSystem.Application.ServiceControllers;
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Web.Api.Controllers
{
    [Route(Routes.ControllerAuth)]
    public class AuthController : Controller
    {
        private readonly IAuthService<BaseResponse, 
                                      RequestRegister, RequestLogin, RequestEmailConfirm, 
                                      string, 
                                      string> authService;

        public AuthController(IAuthService<BaseResponse, 
                                           RequestRegister, RequestLogin, RequestEmailConfirm, 
                                           string, 
                                           string> authService)
        {
            this.authService = authService;
        }

        [HttpPost(Routes.Register)]
        public async Task<IActionResult> RegisterAsync([FromBody]RequestRegister request) => 
            await this.Take(request, authService.RegisterUserAsync);

        [HttpPost(Routes.Login)]
        public async Task<IActionResult> LoginAsync([FromBody] RequestLogin request) =>
            await this.Take(request, authService.LoginUserAsync);

        [HttpPost(Routes.SendConfirmMessage)]
        public async Task<IActionResult> SendConfirmMassageAsync([Required]string id_user) =>
            await this.Take(id_user, 
                            Routes.ConfirmEmail, Routes.ControllerAuth,
                            authService.SendNewConfirmMessageEmailAsync);

        [Route(Routes.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync(RequestEmailConfirm request) =>
            await this.Take(request, authService.EmailConfirmAsync);

        [Route(Routes.Logout)]
        public async Task<IActionResult> LogoutAsync([Required]string id_user) =>
            await this.Take(id_user, authService.LogoutUserAsync);

        [Route(Routes.Refresh)]
        public async Task<IActionResult> RefreshTokenAsync([Required]string refresh) =>
            await this.Take(refresh, authService.RefreshTokenAsync);
    }
}
