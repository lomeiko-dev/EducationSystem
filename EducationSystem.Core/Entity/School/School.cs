
namespace EducationSystem.Core.Entity.School
{
    public class School : BaseEntity
    {
        public string NameSchool { get; set; }
        public string TypeSchool { get; set; }
        public string Id_Director { get; set; }
        public string NumberSchool { get; set; }
        public string SchoolAddress { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
    }
}
