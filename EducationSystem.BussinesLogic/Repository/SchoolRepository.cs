
using EducationSystem.Application.Repository;
using EducationSystem.Core.Entity.School;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.BussinesLogic.Repository
{
    public class SchoolRepository : IBaseCrud<bool, School, string>
    {
        private readonly ApplicationContext applicationContext;
        public SchoolRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(School entity)
        {
            if (entity == null)
                return false;

            await applicationContext.Schools.AddAsync(entity);
            await applicationContext.SaveChangesAsync();
            return true;
        }

        public async Task<School> GetAsync(string id)
        {
            var school = await applicationContext.Schools.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            return school;
        }

        public async Task<IEnumerable<School>> GetPageAsync(int skip, int take)
        {
            var schools = await applicationContext.Schools.Skip(skip).Take(take).ToListAsync();
            return schools;
        }

        public async Task<bool> UpdateAsync(string id, School entity)
        {
            var school = await applicationContext.Schools.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (school == null)
                return false;

            school.NameSchool = entity.NameSchool;
            school.SchoolAddress = entity.SchoolAddress;
            school.NumberSchool = entity.NumberSchool;
            school.TypeSchool = entity.TypeSchool;
            school.Description = entity.Description;
            school.Id_Director = entity.Id_Director;
            school.IsClosed = entity.IsClosed;
            school.DateUpdate = DateTime.Now;

            await applicationContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var school = await applicationContext.Schools.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (school == null)
                return false;

            applicationContext.Remove(school);
            await applicationContext.SaveChangesAsync();
            return true;
        }
    }
}
