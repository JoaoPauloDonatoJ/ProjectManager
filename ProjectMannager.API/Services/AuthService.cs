using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Common;
using ProjectMannager.API.Data;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Repositories;
using ProjectMannager.API.Repositories.Interfaces;

namespace ProjectMannager.API.Services
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _context;
        public readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        public AuthService(AppDbContext context, ITokenService tokenService, IUserRepository userRepository) 
        {
            _context = context;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterDto registerDto)
        {
            //if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                return ServiceResult.Failure("Email já se encontra em uso !");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash
            };

            //_context.Users.Add(user);
            await _userRepository.AddAsync(user);
            //await _context.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();

            return ServiceResult.Ok("Usuário registrado com sucesso !");
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return ServiceResult<AuthResponseDto>.Failure("Email ou senha inválidos !");
            }

            var token = _tokenService.GenerateToken(user);
            var responseData = new AuthResponseDto(token, user.UserName, user.Email);

            return ServiceResult<AuthResponseDto>.Ok(responseData, "Login efetuado com sucesso !");
        }


    }
}
