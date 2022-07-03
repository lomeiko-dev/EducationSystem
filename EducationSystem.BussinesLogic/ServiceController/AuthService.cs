using EducationSystem.Application.ServiceControllers;
using EducationSystem.BussinesLogic.ExternalService;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.JWT;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EducationSystem.BussinesLogic.ServiceController
{
    public class AuthService : IAuthService<BaseResponse, RequestRegister, RequestLogin, RequestEmailConfirm, string, string>
    {
        private readonly EmailService emailService;

        private readonly OptionsAnswer optionsAnswer;
        private readonly OptionsBaseAnswer optionsBaseAnswer;
        private readonly JwtManager generateJwtToken;
        private readonly UserManager<User> userManager;

        private readonly IUrlHelper urlHelper;

        public AuthService(EmailService emailService,
                           IOptions<OptionsAnswer> optionsAnswer,
                           IOptions<OptionsBaseAnswer> optionsBaseAnswer,
                           UserManager<User> userManager,
                           JwtManager generateJwtToken,
                           IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor)
        {
            this.emailService = emailService;
            this.optionsAnswer = optionsAnswer.Value;
            this.optionsBaseAnswer = optionsBaseAnswer.Value;
            this.generateJwtToken = generateJwtToken;
            this.userManager = userManager;

             urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task<BaseResponse> RegisterUserAsync(RequestRegister request)
        {
            // create entity
            var user = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.NumberPhone,
                DateBirthday = new DateTime(int.Parse(request.DateYearBirth), 
                                            int.Parse(request.DateMonthBirth), 
                                            int.Parse(request.DateDayBirth)),
                HomeAddress = request.HomeAddress
            };

            // registration
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new BaseResponse(result.Errors.Select(x => x.Description), 400);

            // result
            return new BaseResponse(optionsAnswer.RegistrationSuccessfullyMessage, 200);
        }

        public async Task<BaseResponse> LoginUserAsync(RequestLogin request)
        {
            // find user by email address
            var user = await userManager.FindByEmailAsync(request.Login);
                if (user == null)
                    return new BaseResponse(optionsAnswer.UserNotFound, 404);

            // check password
            var resultChekPassword = await userManager.CheckPasswordAsync(user, request.Password);
            if (!resultChekPassword)
                return new BaseResponse(optionsAnswer.InvalidPassword, 400);

            // check confirm email
            var resultCheckEmailConfirm = await userManager.IsEmailConfirmedAsync(user);
            if (!resultCheckEmailConfirm)
                return new BaseResponse(optionsAnswer.EmailNotConfirm, 400);

            // return jwt tokens
            return new BaseResponse(await GenerateJwtToken(user), 200);
        }

        public async Task<BaseResponse> SendNewConfirmMessageEmailAsync(string id_user, string actionEmailConfirm, string controller)
        {
            // find user by id
            var user = await userManager.FindByIdAsync(id_user);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            // generate url
            string url = urlHelper.Action(actionEmailConfirm, controller,
                                          new { id_user = user.Id.ToString(), token = token },
                                          urlHelper.ActionContext.HttpContext.Request.Scheme).Replace("%2F", "/");

            emailService.InitializeMime(user.Email, optionsAnswer.ConfirmEmailMassage.Replace("{fullname}", user.FullName)
                                                                                     .Replace("{url}", url));
            await emailService.Send();

            return new BaseResponse(optionsAnswer.SendMailSuccessful.Replace("{address}", user.Email), 200);
        }

        public async Task<BaseResponse> EmailConfirmAsync(RequestEmailConfirm request)
        {
            // find user by id from request
            var user = await userManager.FindByIdAsync(request.Id_user);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            var resultConfirmEmail = await userManager.IsEmailConfirmedAsync(user);
            if (resultConfirmEmail)
                return new BaseResponse(optionsAnswer.Emailconfirmed, 400);

            // check tokens
            var result = await userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
                return new BaseResponse(optionsBaseAnswer.NotValid.Replace("{object}", "token"), 400);

            return new BaseResponse(optionsAnswer.Emailconfirmed, 200);
        }

        public async Task<BaseResponse> LogoutUserAsync(string id_user)
        {
            // find token by user id
            var user = await userManager.FindByIdAsync(id_user);
            if (user == null)
                return new BaseResponse(optionsBaseAnswer.NotFound.Replace("{object}", "user"), 404);

            user.Refresh_token = null;
            await userManager.UpdateAsync(user);
            return new BaseResponse(optionsBaseAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> RefreshTokenAsync(string request)
        {
            var principal = generateJwtToken.GetPrincipal(request);

            // find user by id from token
            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.Refresh_token != request)
                return new BaseResponse(optionsBaseAnswer.NotValid.Replace("{object}", "client request"), 400);

            // create tokens
            return new BaseResponse(await GenerateJwtToken(user), 200);
        }

        #region Help-methods

        private async Task<object> GenerateJwtToken(User user)
        {
            // create tokens
            string accessToken = generateJwtToken.GenerateToken(new List<Claim> { new Claim("Id", user.Id.ToString()),
                                                                                  new Claim(ClaimTypes.Name, user.UserName),
                                                                                  new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                                                                                  new Claim(JwtRegisteredClaimNames.Sub, user.FullName)},
                                                                GeneratorType.Acceess);

            string refreshToken = generateJwtToken.GenerateToken(new List<Claim> { new Claim(ClaimTypes.Name, user.UserName)},
                                                                 GeneratorType.Refresh);

            user.Refresh_token = refreshToken;
            await userManager.UpdateAsync(user);

            return new { accessToken = accessToken, refreshToken = refreshToken };
        }
        #endregion
    }
}
