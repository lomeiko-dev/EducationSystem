using EducationSystem.Core.Entity.User;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Infrastructure.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ConfirmToken> ConfirmEmailTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
