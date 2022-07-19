
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestSchool
    {
        [Required]
        public string TypeSchool { get; set; }

        public string Id_Director { get; set; }

        [Required(ErrorMessage = "Укажите имя школы")]
        public string NameSchool { get; set; }

        [Required(ErrorMessage = "Укажите имя номер школы")]
        public string NumberSchool { get; set; }

        [Required(ErrorMessage = "Укажите адрес школы")]
        public string SchoolAddress { get; set; }

        [Required(ErrorMessage = "Укажите описание школы")]
        public string Description { get; set; }

        [Required]
        public bool Is_Closed { get; set; }
    }
}
