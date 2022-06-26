using EducationSystem.Application.Repository.User;
using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Generators;
using EducationSystem.Helper.Hash;
using EducationSystem.Helper.Options;
using EducationSystem.Helper.Response;
using EducationSystem.Helper.Validators;
using Microsoft.Extensions.Options;

namespace EducationSystem.BussinesLogic.Managers
{
    public class UserManager
    {
        private readonly IUserRepository<bool, User, string> userRepository;
        private readonly IConfirmTokenRepository<bool, ConfirmToken, string> confirmToken;

        private readonly OptionsAnswer optionsAnswer;
        private readonly GenerateToken generateToken;
        private readonly ValidatePassword validatePassword;
        private readonly HashService hashService;

        public UserManager(IUserRepository<bool, User, string> userRepository,
                           IConfirmTokenRepository<bool, ConfirmToken, string> confirmToken,
                           IOptions<OptionsAnswer> optionsAnswer,
                           ValidatePassword validatePassword,
                           GenerateToken generateToken,
                           HashService hashService)
        {
            this.userRepository = userRepository;
            this.confirmToken = confirmToken;
            this.optionsAnswer = optionsAnswer.Value;
            this.validatePassword = validatePassword;
            this.generateToken = generateToken;
            this.hashService = hashService;
        }
        public async Task<BaseResponse> CreateAsync(User user, string password)
        {
            // validate email
            var resultChekEmail = await userRepository.FindUserByEmail(user.Email);
            if (resultChekEmail != null)
                return new BaseResponse(optionsAnswer.EmailExistsMessage, 400, false);

            // validate password
            var resultValidatePassword = await validatePassword.ValidateAsync(password);
            if (!resultValidatePassword.Succeded)
                return resultValidatePassword;

            // correct entity
            user.Password_Hash = await hashService.HashMD5Async(password);
            user.IsEmailConfirm = false;

            // add db
            await userRepository.CreateAsync(user); // add user in db

            return new BaseResponse(true);
        }

        public async Task<bool> IsValidPasswordAsync(User user, string password)
        {
            if (user == null)
                return false;

            return user.Password_Hash == await hashService.HashMD5Async(password) ? true : false;
        }

        public async Task<string> GenerateConfirmationEmailTokenAsync(User user)
        {
            if (user == null)
                return null;

            // find user by id
            var resultFindUser = await userRepository.GetAsync(user.Id.ToString());
            if (resultFindUser == null)
                return null;

            // search confirm token by user id
            var resultFindConfirmToken = await confirmToken.GetAsync(user.Id.ToString());
            if (resultFindConfirmToken != null)
                await confirmToken.DeleteAsync(resultFindConfirmToken); // deleted

            // generate new token
            var token = await generateToken.GenerateAsync();
            await confirmToken.CreateAsync(new ConfirmToken { Id_user = user.Id.ToString(), Token = token });

            return token;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await userRepository.FindUserByEmail(email);
            return user;
        }
        public async Task<User> GetUserByNumberAsync(string number)
        {
            var user = await userRepository.FindUserByNumber(number);
            return user;
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await userRepository.GetAsync(id);
            return user;
        }

        public async Task<BaseResponse> ConfirmEmailByTokenAsync(User user, string token)
        {
            if (user == null)
                return new BaseResponse(optionsAnswer.BadRequest, 400, false);

            // find user
            var resultFindUser = await userRepository.GetAsync(user.Id.ToString());
            if (resultFindUser == null)
                return new BaseResponse(optionsAnswer.NotFound.Replace("{object}", "user"), 404, false);

            // find token by user id
            var emailToken = await confirmToken.GetAsync(user.Id.ToString());
            if (emailToken == null)
                return new BaseResponse(optionsAnswer.NotFound.Replace("{object}", "token"), 404, false);

            // comparisons tokens
            if (emailToken.Token != token)
                return new BaseResponse(optionsAnswer.NotValid.Replace("object", "token"), 400, false);

            // change user
            user.IsEmailConfirm = true;
            await userRepository.UpdateAsync(user, user.Id.ToString());

            // delete confirm token
            await confirmToken.DeleteAsync(emailToken);
            return new BaseResponse(true);
        }
    }
}
