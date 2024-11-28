using System.Security.Claims;
using UserService.BLL.DTO.Request;
using UserService.BLL.DTO.Response;

namespace UserService.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
        public Task<TokenResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        public Task<TokenResponse> RefreshTokenAsync(TokenRequest request, CancellationToken cancellationToken = default);
        public Task<Guid> GetMyIdByJwtAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
        public Task<UserResponse> GetMyProfileByJwtAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
        public Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        public Task<UserResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
