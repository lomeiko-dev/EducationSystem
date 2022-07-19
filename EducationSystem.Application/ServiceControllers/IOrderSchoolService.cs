
namespace EducationSystem.Application.ServiceControllers
{
    public interface IOrderSchoolService<TResponse, TRequest, TUpdate, TId>
    {
        public Task<TResponse> CreateOrderAsync(TRequest request);
        public Task<TResponse> GetOrderAsync(TId id_user);
        public Task<TResponse> GetOrdersPageAsync(int skip, int take);
        public Task<TResponse> UpdateOrderAsync(TId id, TUpdate request);
        public Task<TResponse> DeleteOrderAsync(TId id);
    }
}
