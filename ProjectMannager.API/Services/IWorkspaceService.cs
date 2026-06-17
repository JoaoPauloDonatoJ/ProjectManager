using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;

namespace ProjectMannager.API.Services
{
    public interface IWorkspaceService
    {
        Task<ServiceResult<WorkspaceResponseDto>> CreateAsync(CreateWorkspaceDto dto, int userId);
        Task<ServiceResult<IEnumerable<WorkspaceResponseDto>>> GetUserWorkspacesAsync(int userId);
    }
}
