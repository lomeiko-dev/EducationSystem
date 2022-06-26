
using EducationSystem.Application.Repository.User;
using EducationSystem.Core.Entity.User;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.BussinesLogic.Repository
{
    public class ConfirmEmailTokenRepository : IConfirmTokenRepository<bool, ConfirmToken, string>
    {
        private readonly ApplicationContext applicationContext;

        public ConfirmEmailTokenRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(ConfirmToken entity)
        {
            if (entity == null)
                return false;

            await applicationContext.ConfirmEmailTokens.AddAsync(entity);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<ConfirmToken> GetAsync(string id_user)
        {
            var token = await applicationContext.ConfirmEmailTokens.FirstOrDefaultAsync(x => x.Id_user == id_user);
            return token;
        }

        public async Task DeleteAsync(ConfirmToken entity)
        {
            applicationContext.ConfirmEmailTokens.Remove(entity);
            await applicationContext.SaveChangesAsync();
        }
    }
}
