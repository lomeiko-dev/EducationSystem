
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

        [Required(ErrorMessage = "DateDayBirth is null")]
        public string DateDayBirth { get; set; }

        [Required(ErrorMessage = "DateMonthBirth is null")]
        public string DateMonthBirth { get; set; }

        [Required(ErrorMessage = "DateYearBirth is null")]
        public string DateYearBirth { get; set; }
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Укажите почту")]
        [EmailAddress(ErrorMessage = "Поле электронной почты не является действительным адресом электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
