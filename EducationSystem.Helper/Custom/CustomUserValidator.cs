using EducationSystem.Core.Entity.User;
using EducationSystem.Helper.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EducationSystem.Helper.Custom
{
    public class CustomUserValidator : IUserValidator<User>
    {
        private readonly OptionsCustomValidateUser optionsValidateUser;

        public CustomUserValidator(IOptions<OptionsCustomValidateUser> optionsValidateUser)
        {
            this.optionsValidateUser = optionsValidateUser.Value;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            return await Task.Run(async() =>
            {
                var errors = new List<IdentityError>();

                if (optionsValidateUser.NickNameUseFormat)
                {
                    var fullname = user.FullName.Split(" ");
                    if (fullname.Length != 3)
                        errors.Add(new IdentityError { Description = "ФИО составлено не верно. Верный формат - [Фамилия Имя Отчество]." });
                }

                if (optionsValidateUser.NickNameDontUseSymbol)
                {
                    var resultCheckSymbol = user.FullName.ToList().Where(x => char.IsNumber(x) == true || char.IsSymbol(x) == true).ToList();
                    if (resultCheckSymbol.Count > 0)
                        errors.Add(new IdentityError { Description = "ФИО не должно содержать цифры и символы." });
                }
                
                return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
            });
        }
    }
}
