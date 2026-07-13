using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Data;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;


namespace ProjectMannager.API.Repositories.Implementations
{
    public class ColumnRepository(AppDbContext context) : Repository<Column>(context), IColumnRepository
    {
        public async Task<IEnumerable<Column>> GetByBoardIdAsync(int boardId)
        {
            return await _context.Columns
                .Include(c => c.Board)
                .Where(c => c.BoardId == boardId)
                .ToListAsync();
        }
    }
}

