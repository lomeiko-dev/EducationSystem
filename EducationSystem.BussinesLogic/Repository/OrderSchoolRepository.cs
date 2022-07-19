using EducationSystem.Application.Repository;
using EducationSystem.Core.Entity.School;
using EducationSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationSystem.BussinesLogic.Repository
{
    public class OrderSchoolRepository : IBaseCrud<bool, OrderSchool, string>
    {
        private readonly ApplicationContext applicationContext;

        public OrderSchoolRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(OrderSchool entity)
        {
            if (entity == null)
                return false;

            await applicationContext.OrderSchools.AddAsync(entity);
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<OrderSchool> GetAsync(Expression<Func<OrderSchool, bool>> func)
        {
            var order = await applicationContext.OrderSchools.FirstOrDefaultAsync(func);
            return order;
        }

        public async Task<IEnumerable<OrderSchool>> GetPageAsync(int skip, int take)
        {
            var orders = await applicationContext.OrderSchools.Skip(skip).Take(take).ToListAsync();
            return orders;
        }

        public async Task<bool> UpdateAsync(string id, OrderSchool entity)
        {
            var order = await applicationContext.OrderSchools.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (order == null)
                return false;

            order.Message = entity.Message;
            order.DateUpdate = DateTime.Now;
            await applicationContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = await applicationContext.OrderSchools.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (order == null)
                return false;

            applicationContext.OrderSchools.Remove(order);
            return true;
        }
    }
}
