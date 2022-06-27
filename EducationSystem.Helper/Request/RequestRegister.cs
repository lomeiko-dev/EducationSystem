
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestRegister
    {
        [Required(ErrorMessage = "Укажите ФИО")]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string NumberPhone { get; set; }
        public int Age { get; set; }
        public string DateDayBirth { get; set; }
        public string DateMonthBirth { get; set; }
        public string DateYearBirth { get; set; }
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
