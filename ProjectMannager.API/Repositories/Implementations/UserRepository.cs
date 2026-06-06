using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Data;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Repositories.Implementations
{
    // Herdamos a lógica base e assinamos o contrato específico
    public class UserRepository : Repository<User>, IUserRepository
    {
        // O "base(context)" repassa o DbContext para a classe Repository pai
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        // Busca o usuário por e-mail ou retorna null se não encontrar
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Verifica de forma performática se o e-mail já existe (retorna apenas true/false)
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}