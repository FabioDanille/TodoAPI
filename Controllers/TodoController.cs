using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoAPI.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class TodoController : ControllerBase
    {
        // GET
        [HttpGet, Route("todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context
        )
        {
            List<TodoModel> todos = await context.Todos.AsNoTracking().ToListAsync();
            return Ok(todos); 
        }

    }
}	