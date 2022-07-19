
using EducationSystem.Core.Role;

namespace EducationSystem.Helper.Request
{
    public class RequestValidateAddedRole
    {
        public List<string> IssuerRoles { get; set; }
        public List<string> Roles { get; set; }
    }
}
