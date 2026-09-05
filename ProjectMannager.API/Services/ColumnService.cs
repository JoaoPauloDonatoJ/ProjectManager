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

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return ServiceResult<ColumnResponseDto>.Failure("O nome da coluna é obrigatório e não pode conter apenas espaços.");
            }

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

        public async Task<ServiceResult<IEnumerable<ColumnResponseDto>>> GetColumnsBoardIdAsync(int boardId, int userId)
        {
            var board = await _boardRepository.GetByIdWithWorkspaceAsync(boardId);

            if (board == null)
            {
                return ServiceResult<IEnumerable<ColumnResponseDto>>.Failure("Board não encontrado.");
            }

            // 2. 🔐 Validação Crítica de Segurança
            if (board.Workspace.UserId != userId)
            {
                return ServiceResult<IEnumerable<ColumnResponseDto>>.Failure("Você não tem permissão para acessar os Boards deste Workspace.");
            }

            var columns = await _columnRepository.GetByBoardIdAsync(boardId);

            var response = columns.Select(c => new ColumnResponseDto(c.Id, c.Name, c.Position, c.BoardId));

            return ServiceResult<IEnumerable<ColumnResponseDto>>.Ok(response);
        }

        public async Task<ServiceResult<ColumnResponseDto>> UpdateColumnAsync(int columnId, UpdateColumnDto dto, int userId)
        {
            var column = await _columnRepository.GetByIdWithBoardAndWorkspaceAsync(columnId);

            if (column == null)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Coluna não encontrada.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return ServiceResult<ColumnResponseDto>.Failure("O nome da coluna é obrigatório.");
            }

            if (column.Board.Workspace.UserId != userId)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Você não tem permissão para atualizar esta coluna.");
            }

            var positionBefore = column.Position;
            var positionAfter = dto.Position;

            // Se a posição não mudou, apenas atualiza o nome
            if (positionBefore == positionAfter)
            {
                column.Name = dto.Name;
                _columnRepository.Update(column);
                await _columnRepository.SaveChangesAsync();

                return ServiceResult<ColumnResponseDto>.Ok(new ColumnResponseDto(column.Id, column.Name, column.Position, column.BoardId));
            }

            // Busca todas as colunas do Board
            var boardColumns = (await _columnRepository.GetByBoardIdAsync(column.BoardId)).ToList();

            if (positionAfter < 1 || positionAfter > boardColumns.Count)
            {
                return ServiceResult<ColumnResponseDto>.Failure("A posição da coluna é inválida.");
            }

            // Reordena apenas as colunas vizinhas (excluindo a própria coluna que está sendo movida)
            foreach (var boardColumn in boardColumns.Where(c => c.Id != columnId))
            {
                // Cenário A: Mover para CIMA (ex: de 4 para 2)
                if (positionAfter < positionBefore)
                {
                    if (boardColumn.Position >= positionAfter && boardColumn.Position < positionBefore)
                    {
                        boardColumn.Position++;
                        _columnRepository.Update(boardColumn);
                    }
                }
                // Cenário B: Mover para BAIXO (ex: de 2 para 4)
                else if (positionAfter > positionBefore)
                {
                    if (boardColumn.Position > positionBefore && boardColumn.Position <= positionAfter)
                    {
                        boardColumn.Position--;
                        _columnRepository.Update(boardColumn);
                    }
                }
            }

            // Por fim, atualiza a coluna alvo
            column.Name = dto.Name;
            column.Position = dto.Position;
            _columnRepository.Update(column);

            await _columnRepository.SaveChangesAsync();

            return ServiceResult<ColumnResponseDto>.Ok(new ColumnResponseDto(
                column.Id,
                column.Name,
                column.Position,
                column.BoardId
            ));
        }

        public async Task<ServiceResult<ColumnResponseDto>> GetColumnByIdAsync(int columnId, int userId)
        {
            var column = await _columnRepository.GetByIdWithBoardAndWorkspaceAsync(columnId);

            if (column == null)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Coluna não encontrada.");
            }
            // 2. Validação de Segurança
            if (column.Board.Workspace.UserId != userId)
            {
                return ServiceResult<ColumnResponseDto>.Failure("Você não tem permissão para acessar esta coluna.");
            }

            var response = new ColumnResponseDto(
                column.Id,
                column.Name,
                column.Position,
                column.BoardId
            );
            return ServiceResult<ColumnResponseDto>.Ok(response);
        }
    }
}
