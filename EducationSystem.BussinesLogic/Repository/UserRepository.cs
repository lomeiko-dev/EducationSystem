
using EducationSystem.Application.Repository.User;
using EducationSystem.Core.Entity.User;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.BussinesLogic.Repository
{
    public class UserRepository : IUserRepository<bool, User, string>
    {
        private readonly ApplicationContext applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(User entity)
        {
            if (entity == null)
                return false;

            await applicationContext.AddAsync(entity);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetAsync(string id)
        {
            var user = await applicationContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetPagesAsync(int skip, int take)
        {
            var users = await applicationContext.Users.Skip(skip).Take(take).ToListAsync();
            return users;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await applicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<bool> UpdateAsync(User entity, string id)
        {
            if (entity == null)
                return false;

            var user = await applicationContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (user == null)
                return false;
               

            user.FullName = entity.FullName;
            user.Phone_Number = entity.Phone_Number;
            user.Password_Hash = entity.Password_Hash;
            user.IsEmailConfirm = entity.IsEmailConfirm;
            user.Age = entity.Age;

            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(User entity)
        {
            applicationContext.Users.Remove(entity);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<User> FindUserByNumber(string number)
        {
            var user = await applicationContext.Users.FirstOrDefaultAsync(x => x.Phone_Number == number);
            return user;
        }
    }
}
