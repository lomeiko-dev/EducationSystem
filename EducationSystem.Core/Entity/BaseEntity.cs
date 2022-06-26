
namespace EducationSystem.Core.Entity
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreate = DateTime.Now;
            DateUpdate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
