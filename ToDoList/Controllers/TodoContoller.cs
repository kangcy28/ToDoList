using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private readonly TodoManager _todoManager;

        public TodoController(TodoManager todoManager)
        {
            _todoManager = todoManager;
        }

        /// <summary>
        /// Retrieves all todo items
        /// </summary>
        /// <returns>A collection of todo items</returns>
        /// <response code="200">Returns the list of todo items</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all todos",
            Description = "Retrieves all todo items from the database",
            OperationId = "GetTodos",
            Tags = new[] { "Todos" }
        )]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var todos = await _todoManager.GetAllTodosAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Retrieves a specific todo item by ID
        /// </summary>
        /// <param name="id">The ID of the todo item</param>
        /// <returns>The requested todo item</returns>
        /// <response code="200">Returns the requested todo item</response>
        /// <response code="404">If the todo item is not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get todo by ID",
            Description = "Retrieves a specific todo item by its ID",
            OperationId = "GetTodoById",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(200, "The todo item was successfully retrieved", typeof(Todo))]
        [SwaggerResponse(404, "Todo item not found")]
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

        /// <summary>
        /// Creates a new todo item
        /// </summary>
        /// <param name="request">The todo item to create</param>
        /// <returns>The created todo item</returns>
        /// <response code="201">Returns the newly created todo item</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create new todo",
            Description = "Creates a new todo item",
            OperationId = "CreateTodo",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(201, "The todo item was successfully created", typeof(Todo))]
        [SwaggerResponse(400, "Invalid input")]
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

        /// <summary>
        /// Updates an existing todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to update</param>
        /// <param name="request">The updated todo item data</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the todo item was successfully updated</response>
        /// <response code="404">If the todo item was not found</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update todo",
            Description = "Updates an existing todo item with new title and description",
            OperationId = "UpdateTodo",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(204, "Todo item successfully updated")]
        [SwaggerResponse(404, "Todo item not found")]
        [SwaggerResponse(400, "Invalid input")]
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

        /// <summary>
        /// Deletes a specific todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the todo item was successfully deleted</response>
        /// <response code="404">If the todo item was not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete todo",
            Description = "Deletes a specific todo item by its ID",
            OperationId = "DeleteTodo",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(204, "Todo item successfully deleted")]
        [SwaggerResponse(404, "Todo item not found")]
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

        /// <summary>
        /// Toggles the completion status of a todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to toggle</param>
        /// <returns>The new completion status</returns>
        /// <response code="200">Returns the new completion status</response>
        /// <response code="404">If the todo item was not found</response>
        [HttpPatch("{id}/toggle")]
        [SwaggerOperation(
            Summary = "Toggle todo status",
            Description = "Toggles the completion status of a specific todo item",
            OperationId = "ToggleTodoStatus",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(200, "Todo status successfully toggled", typeof(object))]
        [SwaggerResponse(404, "Todo item not found")]
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

        /// <summary>
        /// Saves all todo items to a file
        /// </summary>
        /// <param name="filePath">Optional custom file path for saving todos</param>
        /// <returns>Success message if saved successfully</returns>
        /// <response code="200">If todos were successfully saved</response>
        /// <response code="400">If there are no todos to save</response>
        /// <response code="500">If there was an error saving the file</response>
        [HttpPost("save")]
        [SwaggerOperation(
            Summary = "Save todos to file",
            Description = "Saves all todo items to a file on disk",
            OperationId = "SaveToFile",
            Tags = new[] { "File Operations" }
        )]
        [SwaggerResponse(200, "Todos successfully saved to file")]
        [SwaggerResponse(400, "No todos to save")]
        [SwaggerResponse(500, "Error saving todos to file")]
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

        /// <summary>
        /// Loads todo items from a file
        /// </summary>
        /// <param name="filePath">Optional custom file path for loading todos</param>
        /// <returns>Success message if loaded successfully</returns>
        /// <response code="200">If todos were successfully loaded</response>
        /// <response code="400">If the file is empty or invalid</response>
        /// <response code="500">If there was an error reading the file</response>
        [HttpPost("load")]
        [SwaggerOperation(
            Summary = "Load todos from file",
            Description = "Loads todo items from a file on disk",
            OperationId = "LoadFromFile",
            Tags = new[] { "File Operations" }
        )]
        [SwaggerResponse(200, "Todos successfully loaded from file")]
        [SwaggerResponse(400, "No todos found in file")]
        [SwaggerResponse(500, "Error loading todos from file")]
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

    /// <summary>
    /// Request model for creating a new todo item
    /// </summary>
    public class TodoCreateRequest
    {
        /// <summary>
        /// The title of the todo item
        /// </summary>
        /// <example>Complete project documentation</example>
        [SwaggerSchema(Description = "The title of the todo item")]
        public string Title { get; set; }

        /// <summary>
        /// The description of the todo item
        /// </summary>
        /// <example>Write comprehensive documentation for the API project</example>
        [SwaggerSchema(Description = "The description of the todo item")]
        public string Description { get; set; }
    }
    /// <summary>
    /// Request model for updating an existing todo item
    /// </summary>
    public class TodoUpdateRequest
    {
        /// <summary>
        /// The new title of the todo item
        /// </summary>
        /// <example>Updated project documentation</example>
        [SwaggerSchema(Description = "The new title of the todo item")]
        public string Title { get; set; }

        /// <summary>
        /// The new description of the todo item
        /// </summary>
        /// <example>Update the API documentation with new endpoints</example>
        [SwaggerSchema(Description = "The new description of the todo item")]
        public string Description { get; set; }
    }
}