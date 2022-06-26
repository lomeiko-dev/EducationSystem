
namespace EducationSystem.Application.ServiceControllers
{
    public interface IAuthService<TResponse, TRequestRegister, TRequestLogin, TRequestEmailConfirm, TRefreshToken, TId>
    {
        public Task<TResponse> RegisterUserAsync(TRequestRegister request);
        public Task<TResponse> LoginUserAsync(TRequestLogin request);
        public Task<TResponse> SendNewConfirmMessageEmailAsync(TId id_user, string actionEmailConfirm, string controller);
        public Task<TResponse> EmailConfirmAsync(TRequestEmailConfirm request);
        public Task<TResponse> LogoutUserAsync(TId id_user);
        public Task<TResponse> RefreshTokenAsync(TRefreshToken request);
    }
}
