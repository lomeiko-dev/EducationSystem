
using Microsoft.AspNetCore.Identity;

namespace EducationSystem.Core.Entity.User
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? HomeAddress { get; set; }
        public DateTime DateBirthday { get; set; }
        public string? Id_School { get; set; }
        public string? Id_Class { get; set; }
        public string? Refresh_token { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
