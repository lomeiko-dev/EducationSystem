
namespace EducationSystem.Application.Validate
{
    public interface IValidate<TResponse, TObject>
    {
        public Task<TResponse> ValidateAsync(TObject obj);
    }
}
