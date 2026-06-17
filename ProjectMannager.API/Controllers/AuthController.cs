using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Services;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Data;
using BCrypt.Net;

namespace ProjectMannager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        
        public readonly ITokenService _tokenService;
        public readonly IAuthService _authService;

        public AuthController(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok( new {message = result.Message});
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if(!result.Success)
            {
                return Unauthorized(new { error = result.Message });
            }

            return Ok(result.Data);
        }
    }
}
