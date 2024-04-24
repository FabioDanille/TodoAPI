using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoAPI.ViewModel;

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

        // GET BY ID
        [HttpGet, Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var todos = await context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return todos == null ? NotFound() : Ok(todos);
        }

        // POST
        [HttpPost, Route("todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model
        )
        {
            if(!ModelState.IsValid) return BadRequest();

            TodoModel todo = new TodoModel
            {
                CreationDate = DateTime.Now,
                Done = model.Done,
                Title = model.Title
            };

            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }catch(Exception e)
            {
                return BadRequest();
            }


        }

    }
}	