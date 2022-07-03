
namespace EducationSystem.Application.ServiceControllers
{
    public interface ISchoolService<TResponse, TRequest, TId>
    {
        public Task<TResponse> CreateSchoolAsync(TRequest request);
        public Task<TResponse> GetSchoolAsync(TId id);
        public Task<TResponse> GetSchoolsPageAsync(int skip, int take);
        public Task<TResponse> UpdateSchoolAsync(TId id, TRequest request);
        public Task<TResponse> DeleteSchoolAsync(TId id);
    }
}
