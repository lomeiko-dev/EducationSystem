
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestLogin
    {
        [Required(ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        public string Password { get; set; }
    }
}
