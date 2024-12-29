using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoManager _todoManager;

        public TodoController(TodoManager todoManager)
        {
            _todoManager = todoManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var todos = await _todoManager.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            try
            {
                var todo = await _todoManager.GetTodoByIdAsync(id);
                return Ok(todo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] TodoCreateRequest request)
        {
            try
            {
                var todo = await _todoManager.AddTodoAsync(request.Title, request.Description);
                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
        {
            try
            {
                await _todoManager.UpdateTodoAsync(id, request.Title, request.Description);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                await _todoManager.DeleteTodoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTodoStatus(int id)
        {
            try
            {
                var newStatus = await _todoManager.ToggleTodoStatusAsync(id);
                return Ok(new { isCompleted = newStatus });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveToFile([FromQuery] string filePath = null)
        {
            try
            {
                await _todoManager.SaveToFileAsync(filePath);
                return Ok("Todos saved successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving todos: {ex.Message}");
            }
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadFromFile([FromQuery] string filePath = null)
        {
            try
            {
                await _todoManager.LoadFromFileAsync(filePath);
                return Ok("Todos loaded successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error loading todos: {ex.Message}");
            }
        }
    }

    public class TodoCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class TodoUpdateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}