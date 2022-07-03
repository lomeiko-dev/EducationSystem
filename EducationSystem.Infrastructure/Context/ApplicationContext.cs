using EducationSystem.Core.Entity.School;
using EducationSystem.Core.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Infrastructure.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<School> Schools { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
