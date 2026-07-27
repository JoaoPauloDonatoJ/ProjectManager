using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMannager.API.Services;

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
            return Ok();
        }
    }
}