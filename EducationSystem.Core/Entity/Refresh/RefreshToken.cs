using EducationSystem.Core.Entity;

namespace EducationSystem.Core.Entity.Refresh
{
    public class RefreshToken : BaseEntity
    {
        public string id_user { get; set; }
        public string Refresh { get; set; }
    }
}
