using Microsoft.AspNetCore.Mvc;
using ProjectMannager.API.Services;
using ProjectMannager.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjectMannager.API.Controllers
{
    [ApiController]
    [Route("api/workspaces")]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserWorkspaces()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new { error = "Ocorreu um erro ao tentar obter o ID do usuário." });

            var result = await _workspaceService.GetUserWorkspacesAsync(userId.Value);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new { error = "Ocorreu um erro ao tentar obter o ID do usuário." });

            var result = await _workspaceService.CreateAsync(dto, userId.Value);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            // Retorna HTTP 201 Created
            // 1º parâmetro: Nome do método que busca o recurso por ID
            // 2º parâmetro: Objeto anônimo com o parâmetro de rota que o método de busca espera (id)
            // 3º parâmetro: O objeto com os dados criados (result.Data)
            return CreatedAtAction(nameof(GetWorkspaceById), new { id = result.Data.Id }, result.Data);
        }

        [Authorize]
        [HttpGet("{id:int}")] // Rota necessária para o CreatedAtAction funcionar
        public async Task<IActionResult> GetWorkspaceById(int id)
        {
            return Ok();
        }

        // Método auxiliar privado para centralizar a extração e parsing do ID do Token
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            return int.TryParse(userIdClaim, out var id) ? id : null;
        }
    }
}