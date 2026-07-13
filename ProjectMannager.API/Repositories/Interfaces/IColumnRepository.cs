using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Repositories.Interfaces
{
    public interface IColumnRepository : IRepository<Column>
    {
        Task<IEnumerable<Column>> GetByBoardIdAsync(int boardId);
    }
}
