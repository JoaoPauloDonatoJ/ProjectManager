using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Data;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Repositories.Implementations
{
    public class WorkspaceRepository(AppDbContext context) : Repository<Workspace>(context), IWorkspaceRepository
    {
        public async Task<IEnumerable<Workspace>> GetByUserIdAsync(int userId)
        {
            return await _context.Workspaces
            .Include(w => w.Boards)
            .Where(w => w.UserId == userId)
            .ToListAsync();
        }
        
    }
}
