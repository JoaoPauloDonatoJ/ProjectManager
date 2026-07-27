using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Repositories.Interfaces
{
    public interface IBoardRepository : IRepository<Board>
    {
        Task<IEnumerable<Board>> GetByWorkspaceIdAsync(int workspaceId);

        Task<Board> GetByIdWithWorkspaceAsync(int boardId);
    }
}
