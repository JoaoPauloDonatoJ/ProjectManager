using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMannager.API.Services;
using System.Security.Claims;
using ProjectMannager.API.DTOs;

namespace ProjectMannager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnService _columnService;

        public ColumnController(IColumnService columnService)
        {
            _columnService = columnService;
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetColumnById")] 
        public async Task<IActionResult> GetColumnById(int id)
        {
            var userId = GetUserId();

            var result = await _columnService.GetColumnByIdAsync(id, userId.Value);

            if (!result.Success)
            {
                return BadRequest(new { error = result.Message });
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPut("{columnId:int}")]
        public async Task<IActionResult> UpdateColumnAsync(int columnId, UpdateColumnDto dto)
        {
            var userId = GetUserId();

            if (!userId.HasValue)
                return Unauthorized();

            var result = await _columnService.UpdateColumnAsync(columnId, dto, userId.Value);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
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