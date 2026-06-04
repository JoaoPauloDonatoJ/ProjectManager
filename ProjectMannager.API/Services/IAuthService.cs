using ProjectMannager.API.DTOs;
using ProjectMannager.API.Common;

namespace ProjectMannager.API.Services
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    }
}
