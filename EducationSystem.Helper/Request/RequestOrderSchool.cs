using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestOrderSchool
    {
        [Required]
        public string Id_User { get; set; }
        [Required]
        public string Id_School { get; set; }
        public string Message { get; set; }
    }
}
