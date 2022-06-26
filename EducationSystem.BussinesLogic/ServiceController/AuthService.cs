using EducationSystem.Application.Repository.User;
using EducationSystem.Application.ServiceControllers;
using EducationSystem.BussinesLogic.ExternalService;
using EducationSystem.BussinesLogic.Managers;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Generators;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Request;
using EducationSystem.Helper.Response;
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
        private readonly GenerateJwtToken generateJwtToken;

        private readonly IRefreshRepository<bool, RefreshToken, string> refreshRepository;
        private readonly UserManager userManager;

        private readonly IUrlHelper urlHelper;

        public AuthService(
                           EmailService emailService,
                           IOptions<OptionsAnswer> optionsAnswer,
                           UserManager userManager,
                           GenerateJwtToken generateJwtToken,
                           IRefreshRepository<bool, RefreshToken, string> refreshRepository,
                           IUrlHelperFactory urlHelperFactory,
                           IActionContextAccessor actionContextAccessor)
        {
            this.emailService = emailService;
            this.optionsAnswer = optionsAnswer.Value;
            this.generateJwtToken = generateJwtToken;
            this.refreshRepository = refreshRepository;
            this.userManager = userManager;

             urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task<BaseResponse> RegisterUserAsync(RequestRegister request)
        {
            // create entity
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone_Number = request.NumberPhone,
                Age = request.Age
            };

            // registration
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeded)
                return result;

            return new BaseResponse(optionsAnswer.RegistrationSuccessfullyMessage, 200);
        }

        public async Task<BaseResponse> LoginUserAsync(RequestLogin request)
        {
            // find user by email address
            var user = await userManager.GetUserByEmailAsync(request.Login);
                if (user == null)
                    return new BaseResponse(optionsAnswer.UserNotFound, 404);

            // check password
            var resultChekPassword = await userManager.IsValidPasswordAsync(user, request.Password);
            if (!resultChekPassword)
                return new BaseResponse(optionsAnswer.InvalidPassword, 400);

            // check confirm email
            if(!user.IsEmailConfirm)
                return new BaseResponse(optionsAnswer.EmailNotConfirm, 400);
               
            // return jwt tokens
            return new BaseResponse(await GenerateJwtToken(user), 200);
        }

        public async Task<BaseResponse> SendNewConfirmMessageEmailAsync(string id_user, string actionEmailConfirm, string controller)
        {
            // find user by id
            var user = await userManager.GetUserByIdAsync(id_user);
            if (user == null)
                return new BaseResponse(optionsAnswer.NotFound.Replace("{object}", "user"), 404);

            var token = await userManager.GenerateConfirmationEmailTokenAsync(user);
            // generate url
            string url = urlHelper.Action(actionEmailConfirm, controller,
                                          new { id_user = user.Id.ToString(), token = token },
                                          urlHelper.ActionContext.HttpContext.Request.Scheme).Replace("%2F", "/");

            emailService.InitializeMime(user.Email, optionsAnswer.ConfirmEmailMassage.Replace("{fullname}", user.FullName)
                                                                                     .Replace("{url}", url));
            await emailService.Send();

            return new BaseResponse(optionsAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> EmailConfirmAsync(RequestEmailConfirm request)
        {
            // find user by id from request
            var user = await userManager.GetUserByIdAsync(request.Id_user);
            if (user == null)
                return new BaseResponse(optionsAnswer.NotFound, 404);

            // check tokens
            var result = await userManager.ConfirmEmailByTokenAsync(user, request.Token);
            if (!result.Succeded)
                return result;

            return new BaseResponse(optionsAnswer.Emailconfirmed, 200);
        }

        public async Task<BaseResponse> LogoutUserAsync(string id_user)
        {
            // find token by user id
            var token = await refreshRepository.GetTokenByUserIdAsync(id_user);
            if (token == null)
                return new BaseResponse(optionsAnswer.NotFound.Replace("{object}", "user"), 404);

            // delete token
            await refreshRepository.DeleteTokenAsync(token);

            return new BaseResponse(optionsAnswer.Succeeded, 200);
        }

        public async Task<BaseResponse> RefreshTokenAsync(string request)
        {
            // find token by refresh token from request
            var token = await refreshRepository.GetTokenByRefreshToken(request);
            if (token == null)
                return new BaseResponse(optionsAnswer.NotValid.Replace("{object}", "token"), 404);

            // check validate refresh token
            if (!generateJwtToken.IsValid(token.Refresh))
                return new BaseResponse(optionsAnswer.NotValid.Replace("{object}", "token"), 400);

            // delete token by id
            await refreshRepository.DeleteTokenAsync(token);

            // find user by id from token
            var user = await userManager.GetUserByIdAsync(token.id_user);
            if (user == null)
                return new BaseResponse(optionsAnswer.NotFound.Replace("{object}", "user"), 404);

            // create tokens
            return new BaseResponse(await GenerateJwtToken(user), 200);
        }

        #region Help-methods

        private async Task<object> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                new Claim(JwtRegisteredClaimNames.Sub, user.FullName)
            };

            // create tokens
            string accessToken = generateJwtToken.CreateToken(15, claims.ToList());
            string refreshToken = generateJwtToken.CreateToken(131400);

            var token = await refreshRepository.GetTokenByUserIdAsync(user.Id.ToString());
            if (token != null)
                await refreshRepository.DeleteTokenAsync(token);

            await refreshRepository.CreateTokenAsync(new RefreshToken { Refresh = refreshToken, id_user = user.Id.ToString() });

            return new { accessToken = accessToken, refreshToken = refreshToken };
        }
        #endregion
    }
}
