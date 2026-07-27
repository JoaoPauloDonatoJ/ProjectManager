using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMannager.API.DTOs;
using ProjectMannager.API.Services;
using System.Security.Claims;

namespace ProjectMannager.API.Controllers
{
    [ApiController]
    [Route("api/boards")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;
        private readonly IColumnService _columnService;
        public BoardController(IBoardService boardService, IColumnService columnService)
        {
            _boardService = boardService;
            _columnService = columnService;
        }

        
        [Authorize]
        [HttpPost("{boardId:int}/columns")]
        public async Task<IActionResult> CreateColumn(int boardId, [FromBody] CreateColumnDto dto)
        {
            var userId = GetUserId();

            var result = await _columnService.CreateColumnAsync(dto, boardId, userId.Value);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return CreatedAtRoute(
                "GetColumnById",
                new { id = result.Data.Id },
                result.Data
            );
        }


        [Authorize]
        [HttpGet("{id:int}")] // Rota necessária para o CreatedAtAction funcionar
        public async Task<IActionResult> GetBoardById(int id)
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
