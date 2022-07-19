using EducationSystem.Core.Entity.Role;
using EducationSystem.Core.Entity.User;

namespace EducationSystem.Helper.Request
{
    public class RequestValidateChangeUser
    {
        public User NewUser { get; set; }
        public Role Role { get; set; }
    }
}
