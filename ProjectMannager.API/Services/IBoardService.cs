using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;

namespace ProjectMannager.API.Services
{
    public interface IBoardService
    {
        Task<ServiceResult<BoardResponseDto>> CreateBoardAsync(CreateBoardDto dto,int workspaceId, int userId);
        Task<ServiceResult<IEnumerable<BoardResponseDto>>> GetWorkspaceBoardsAsync(int workspaceId);
    }
}
