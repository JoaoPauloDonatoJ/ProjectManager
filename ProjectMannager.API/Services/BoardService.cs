using ProjectMannager.API.Common;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Services
{
    public class BoardService (IBoardRepository _boardRepository, IUserRepository _userRepository, IWorkspaceRepository _workspaceRepository) : IBoardService
    {
        public async Task<ServiceResult<BoardResponseDto>> CreateBoardAsync(CreateBoardDto dto, int workspaceId, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<BoardResponseDto>.Failure("Usuário não encontrado.");
            }

            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            if (workspace == null)
            {
                return ServiceResult<BoardResponseDto>.Failure("Workspace não encontrado.");
            }

            // 🔐 Validação Crítica de Segurança:
            if (workspace.UserId != userId)
            {
                return ServiceResult<BoardResponseDto>.Failure("Você não tem permissão para criar um Board neste Workspace.");
            }

            var board = new Board
            {
                Name = dto.Name,
                Description = dto.Description,
                WorkspaceId = workspaceId,
                CreatedByName = user.UserName
            };

            await _boardRepository.AddAsync(board);
            await _boardRepository.SaveChangesAsync();

            
            var response = new BoardResponseDto(board.Id, board.Name, board.Description, board.WorkspaceId);
            return ServiceResult<BoardResponseDto>.Ok(response);
        }

        public async Task<ServiceResult<IEnumerable<BoardResponseDto>>> GetWorkspaceBoardsAsync(int workspaceId, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<IEnumerable<BoardResponseDto>>.Failure("Usuário não encontrado.");
            }

            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            if (workspace == null)
            {
                return ServiceResult<IEnumerable<BoardResponseDto>>.Failure("Workspace não encontrado.");
            }

            // 🔐 Validação Crítica de Segurança:
            if (workspace.UserId != userId)
            {
                return ServiceResult<IEnumerable<BoardResponseDto>>.Failure("Você não tem permissão para acessar os Boards deste Workspace.");
            }

            var boards = await _boardRepository.GetByWorkspaceIdAsync(workspaceId);

            //if (!boards.Any())
            //{
            //    return ServiceResult<IEnumerable<BoardResponseDto>>.Failure("Nenhum Board encontrado para este Workspace.");
            //}

            var response = boards.Select(b => new BoardResponseDto(b.Id, b.Name, b.Description, b.WorkspaceId));

            return ServiceResult<IEnumerable<BoardResponseDto>>.Ok(response);
        }
    }
}
