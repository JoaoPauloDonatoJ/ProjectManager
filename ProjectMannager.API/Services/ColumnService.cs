using Azure;
using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Implementations;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Services
{
    public class ColumnService(IBoardRepository _boardRepository, IColumnRepository _columnRepository, IUserRepository _userRepository) : IColumnService
    {
        
        public async Task<ServiceResult<ColumnResponseDto>> CreateColumnAsync(CreateColumnDto dto, int boardId, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Usuário não encontrado.");
            }

            var board = await _boardRepository.GetByIdWithWorkspaceAsync(boardId);

            if(board == null)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Erro ao obter board");
            }

            // 🔐 Validação Crítica de Segurança:
            if (board.Workspace.UserId != userId)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Você não tem permissão para acessar os Boards deste Workspace.");
            }

            var countColumn = await _columnRepository.CountByBoardIdAsync(boardId);

            var newColumn = new Column
            {
                Name = dto.Name,
                BoardId = boardId,
                Position = countColumn, // Set the position based on the existing columns in the board
                CreatedByName = user.UserName
            };

            await _columnRepository.AddAsync(newColumn);
            await _columnRepository.SaveChangesAsync();

            var response = new ColumnResponseDto(newColumn.Id, newColumn.Name, newColumn.Position, newColumn.BoardId);
            return ServiceResult<ColumnResponseDto>.Ok(response);
        }
    }
}
