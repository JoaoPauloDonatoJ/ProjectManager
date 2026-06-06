using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        // Ajustado para bater exatamente com o nome da sua classe concreta
        Task<bool> EmailExistsAsync(string email);
    }
}