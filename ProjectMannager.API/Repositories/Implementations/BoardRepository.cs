using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Data;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Repositories.Implementations
{
    public class BoardRepository(AppDbContext context) : Repository<Board>(context), IBoardRepository
    {
        public async Task<IEnumerable<Board>> GetByWorkspaceIdAsync(int workspaceId)
        {
            return await _context.Boards
            .Include(w => w.Workspace)
            //.Where(w => w.UserId == userId)
            .Where(w => w.WorkspaceId == workspaceId)
            .ToListAsync();
        }
    }
}
