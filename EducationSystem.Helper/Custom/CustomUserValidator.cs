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

                if (optionsValidateUser.isRequireUniqueEmail)
                {
                    var email = await manager.FindByEmailAsync(user.Email);
                    if (email != null)
                        errors.Add(manager.ErrorDescriber.DuplicateEmail(user.Email));
                }

                if (optionsValidateUser.NickNameUseFormat)
                {
                    var fullname = string.Join(" ", user.FullName);
                    if (fullname.Length != 2)
                        errors.Add(new IdentityError { Description = "ФИО составлено не верно. Верный формат - [Фамилия Имя Отчество]." });
                }

                if (optionsValidateUser.NickNameUseSymbol)
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
