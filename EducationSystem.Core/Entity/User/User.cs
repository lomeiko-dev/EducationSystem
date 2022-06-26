
namespace EducationSystem.Core.Entity.User
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password_Hash { get; set; }
        public string Phone_Number { get; set; }
        public int Age { get; set; }
        public bool IsEmailConfirm { get; set; }
        public string? Id_School { get; set; }
        public string? Id_Class { get; set; }
    }
}
