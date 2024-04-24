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
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        // PUT
        [HttpPut, Route("todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id,
            [FromBody] CreateTodoViewModel model
        )
        {
            if(!ModelState.IsValid) return BadRequest();

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if(todo == null) return NotFound();

            try
            {
                todo.Title = model.Title;
                todo.Done = model.Done;
                todo.DateUpdate = DateTime.Now.ToLocalTime();

                context.Todos.Update(todo);
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // DELETE
        [HttpDelete, Route("todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if(todo == null) return NotFound();

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // DELETE ALL
        [HttpDelete, Route("todos")]
        public async Task<IActionResult> DeleteAllAsync(
            [FromServices] AppDbContext context,
            [FromBody] string password
        )
        {
            if (password == "Delete Everything"){
                var todo = await context.Todos.AsNoTracking().ToListAsync();
                context.Todos.RemoveRange(todo);
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            else
            {
                return BadRequest();
            }
                
        }

    }
}	