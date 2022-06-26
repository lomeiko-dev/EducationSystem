
namespace EducationSystem.Application.ServiceControllers
{
    public interface ISchoolController<TResponse, TRequest, TId>
    {
        public Task<TResponse> CreateSchoolAsync(TRequest request);
        public Task<TResponse> GetSchoolAsync(TId id);
        public Task<TResponse> GetSchoolsPageAsync(int skip, int take);
        public Task<TResponse> UpdateSchoolAsync(TRequest request);
        public Task<TResponse> DeleteSchoolAsync(TId id);
        public Task<TResponse> DeleteAllSchoolAsync();
    }
}
