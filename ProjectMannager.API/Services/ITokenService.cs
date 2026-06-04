using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
