
using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Helper.Request
{
    public class RequestRoles
    {
        [Required]
        public List<string> Roles { get; set; }
    }
}
