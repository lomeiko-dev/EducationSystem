
namespace EducationSystem.Application.ServiceControllers
{
    public interface IOrderSchoolService<TResponse, TRequest, TId>
    {
        public Task<TResponse> CreateOrderAsync(TRequest request);
        public Task<TResponse> GetOrderAsync(TRequest request);
        public Task<TResponse> GetOrdersPageAsync(TRequest request);
        public Task<TResponse> UpdateOrderAsync(TRequest request);
        public Task<TResponse> DeleteOrderAsync(TId id);
    }
}
