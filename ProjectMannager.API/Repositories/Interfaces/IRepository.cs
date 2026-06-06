namespace ProjectMannager.API.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Adicionado o "?" para bater com a nulidade da classe concreta Repository
        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveChangesAsync();
    }
}