
using Microsoft.AspNetCore.Identity;

namespace EducationSystem.Core.Entity.Role
{
    public class Role : IdentityRole
    {
        public int Level { get; set; }
    }
}
