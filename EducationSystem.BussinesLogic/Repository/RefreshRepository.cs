using EducationSystem.Application.Repository;
using EducationSystem.Core.Entity.Refresh;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.BussinesLogic.Repository
{
    public class RefreshRepository : IRefreshRepository<bool, RefreshToken, string>
    {
        private readonly ApplicationContext applicationContext;

        public RefreshRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateTokenAsync(RefreshToken entity)
        {
            if (entity == null)
                return false;

            await applicationContext.RefreshTokens.AddAsync(entity);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<RefreshToken> GetTokenByUserIdAsync(string id_user)
        {
            var token = await applicationContext.RefreshTokens.FirstOrDefaultAsync(x => x.id_user == id_user);
            return token;
        }

        public async Task DeleteTokenAsync(RefreshToken entity)
        {
            applicationContext.RefreshTokens.Remove(entity);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetTokenByRefreshToken(string refresh)
        {
            var token = await applicationContext.RefreshTokens.FirstOrDefaultAsync(x => x.Refresh == refresh);
            return token;
        }
    }
}
