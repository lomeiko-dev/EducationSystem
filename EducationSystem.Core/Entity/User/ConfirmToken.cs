
namespace EducationSystem.Core.Entity.User
{
    public class ConfirmToken : BaseEntity
    {
        public string Id_user { get; set; }
        public string Token { get; set; }
    }
}
