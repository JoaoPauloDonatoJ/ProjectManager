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
        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [Authorize]
        [HttpGet("{id:int}")] // Rota necessária para o CreatedAtAction funcionar
        public async Task<IActionResult> GetBoardById(int id)
        {
            return Ok();
        }
    }
}
