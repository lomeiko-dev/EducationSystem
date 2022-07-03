
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestEmailConfirm
    {
        [Required]
        public string Id_user { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
