using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;

namespace ProjectMannager.API.Services
{
    public interface IColumnService
    {
        Task<ServiceResult<ColumnResponseDto>> CreateColumnAsync(CreateColumnDto dto, int boardId, int userId);
        
    }
}
