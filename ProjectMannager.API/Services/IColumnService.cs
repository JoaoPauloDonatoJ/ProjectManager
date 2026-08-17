using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;

namespace ProjectMannager.API.Services
{
    public interface IColumnService
    {
        Task<ServiceResult<ColumnResponseDto>> CreateColumnAsync(CreateColumnDto dto, int boardId, int userId);
        
        Task<ServiceResult<IEnumerable<ColumnResponseDto>>> GetColumnsBoardIdAsync(int boardId, int userId);

        Task<ServiceResult<ColumnResponseDto>> UpdateColumnAsync(int columnId, UpdateColumnDto dto, int userId);

        Task<ServiceResult<ColumnResponseDto>> GetColumnByIdAsync(int columnId, int userId);
    }
}
