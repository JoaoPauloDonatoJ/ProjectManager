using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Repositories.Interfaces
{
    public interface IWorkspaceRepository : IRepository<Workspace>
    {
        Task<IEnumerable<Workspace>> GetByUserIdAsync(int userId);
    }
}
