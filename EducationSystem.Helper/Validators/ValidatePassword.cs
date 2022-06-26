using EducationSystem.Helper.Options;
using EducationSystem.Helper.Response;
using Microsoft.Extensions.Options;

namespace EducationSystem.Helper.Validators
{
    public class ValidatePassword
    {
        private readonly OptionsValidatePassword optionsValidatePassword;

        public ValidatePassword(IOptions<OptionsValidatePassword> optionsValidatePassword)
        {
            this.optionsValidatePassword = optionsValidatePassword.Value;
        }

        public async Task<BaseResponse> ValidateAsync(string password)
        {
            return await Task.Run(() =>
            {
                var errors = new List<string>();

                if (optionsValidatePassword.RequiredLength > password.Length)
                    errors.Add($"Длина пароля должна быть не меньше {optionsValidatePassword.RequiredLength} символов");

                if (optionsValidatePassword.RequireNonAlphabet)
                {
                    var check = password.ToList().Where(x => char.IsLetter(x) == true);
                    if (check.ToList().Count == 0)
                        errors.Add("Пароль должен содержать хотя бы одну алфавитную букву");
                }

                if (optionsValidatePassword.RequireNonNumber)
                {
                    var check = password.ToList().Where(x => char.IsNumber(x) == true);
                    if (check.ToList().Count == 0)
                        errors.Add("Пароль должен содержать хотя бы одну цифру");
                }

                if (optionsValidatePassword.RequireUppercase)
                {
                    var check = password.ToList().Where(x => char.IsUpper(x) == true);
                    if (check.ToList().Count == 0)
                        errors.Add("Пароль должен содержать хотя бы одну цифру");
                }

                if (optionsValidatePassword.RequireLowercase)
                {
                    var check = password.ToList().Where(x => char.IsLower(x) == true);
                    if (check.ToList().Count == 0)
                        errors.Add("Пароль должен содержать хотя бы одну цифру");
                }

                if (errors.Count > 0)
                    return new BaseResponse(errors, 400, false);
                return new BaseResponse(true);
            });
        }
    }
}
