
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestRegister
    {
        [Required(ErrorMessage = "Укажите ФИО")]
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string NumberPhone { get; set; }

        [Required]
        public string DateDayBirth { get; set; }

        [Required]
        public string DateMonthBirth { get; set; }

        [Required]
        public string DateYearBirth { get; set; }

        [Required(ErrorMessage = "Адрес проживания не указан")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "почта не указана")]
        [EmailAddress(ErrorMessage = "Поле электронной почты не является действительным адресом электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
