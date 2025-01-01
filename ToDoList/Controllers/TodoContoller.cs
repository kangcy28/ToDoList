using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using ToDoList.DTOs;
using ToDoList.Models;
using ToDoList.Models.Requests;
using ToDoList.Models.Responses;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private readonly TodoManager _todoManager;
        private readonly ILogger<TodoController> _logger;

        public TodoController(TodoManager todoManager, ILogger<TodoController> logger)
        {
            _todoManager = todoManager;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all todo items
        /// </summary>
        /// <returns>A collection of todo items</returns>
        /// <response code="200">Returns the list of todo items</response>
        [HttpGet]
[SwaggerOperation(Summary = "Get all todo items")]
[SwaggerResponse(StatusCodes.Status200OK, "Todos retrieved successfully", typeof(ApiResponse<IEnumerable<TodoDto>>))]
[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
public async Task<ActionResult<ApiResponse<IEnumerable<TodoDto>>>> GetTodos()
{
    try
    {
        var todoDtos = await _todoManager.GetAllTodosAsync();
        return Ok(ApiResponse<IEnumerable<TodoDto>>.SuccessResponse(todoDtos, "Todos retrieved successfully"));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving todos");
        return StatusCode(StatusCodes.Status500InternalServerError, 
            ApiResponse<IEnumerable<TodoDto>>.ErrorResponse("An error occurred while retrieving todos"));
    }
}

        /// <summary>
        /// Retrieves a specific todo item by ID
        /// </summary>
        /// <param name="id">The ID of the todo item</param>
        /// <returns>The requested todo item</returns>
        /// <response code="200">Returns the requested todo item</response>
        /// <response code="404">If the todo item is not found</response>
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get todo by ID",
            Description = "Retrieves a specific todo item by its ID",
            OperationId = "GetTodoById",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(200, "The todo item was successfully retrieved", typeof(ApiResponse<Todo>))]
        [SwaggerResponse(404, "Todo item not found")]
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TodoDto>>> GetTodo(int id)
        {
            try
            {
                var todo = await _todoManager.GetTodoByIdAsync(id);
                return Ok(ApiResponse<TodoDto>.SuccessResponse(todo, "Todo retrieved successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<Todo>.ErrorResponse($"Todo with ID {id} not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo {Id}", id);
                return StatusCode(500, ApiResponse<Todo>.ErrorResponse("An error occurred while retrieving the todo"));
            }
        }

        /// <summary>
        /// Creates a new todo item
        /// </summary>
        /// <param name="createTodoDto">The todo item to create</param>
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
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<TodoDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TodoDto>>> CreateTodo([FromBody] CreateTodoDto createTodoDto)
        {
            try
            {
                var todoDto = await _todoManager.AddTodoAsync(createTodoDto);
                var response = ApiResponse<TodoDto>.SuccessResponse(todoDto, "Todo created successfully");
                return CreatedAtAction(nameof(GetTodo), new { id = todoDto.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<TodoDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<TodoDto>.ErrorResponse("An error occurred while creating the todo"));
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
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update todo",
            Description = "Updates an existing todo item with new title and description",
            OperationId = "UpdateTodo",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(204, "Todo item successfully updated")]
        [SwaggerResponse(404, "Todo item not found")]
        [SwaggerResponse(400, "Invalid input")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
        {
            try
            {
                UpdateTodoDto updateTodoDto = new UpdateTodoDto
                {
                    Title = request.Title,
                    Description = request.Description,
                };
                await _todoManager.UpdateTodoAsync(id, updateTodoDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"Todo with ID {id} not found"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the todo"));
            }
        }

        /// <summary>
        /// Deletes a specific todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the todo item was successfully deleted</response>
        /// <response code="404">If the todo item was not found</response>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete todo",
            Description = "Deletes a specific todo item by its ID",
            OperationId = "DeleteTodo",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(204, "Todo item successfully deleted")]
        [SwaggerResponse(404, "Todo item not found")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                await _todoManager.DeleteTodoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"Todo with ID {id} not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the todo"));
            }
        }

        /// <summary>
        /// Toggles the completion status of a todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to toggle</param>
        /// <returns>The new completion status</returns>
        /// <response code="200">Returns the new completion status</response>
        /// <response code="404">If the todo item was not found</response>
        [HttpPatch("{id:int}/toggle")]
        [SwaggerOperation(
            Summary = "Toggle todo status",
            Description = "Toggles the completion status of a specific todo item",
            OperationId = "ToggleTodoStatus",
            Tags = new[] { "Todos" }
        )]
        [SwaggerResponse(200, "Todo status successfully toggled", typeof(ApiResponse<object>))]
        [SwaggerResponse(404, "Todo item not found")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> ToggleTodoStatus(int id)
        {
            try
            {
                var newStatus = await _todoManager.ToggleTodoStatusAsync(id);
                return Ok(ApiResponse<object>.SuccessResponse(new { isCompleted = newStatus }, "Todo status toggled successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"Todo with ID {id} not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling todo status {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while toggling the todo status"));
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
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> SaveToFile([FromQuery] string? filePath = null)
        {
            try
            {
                await _todoManager.SaveToFileAsync(filePath);
                return Ok(ApiResponse<object>.SuccessResponse(null, "Todos saved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving todos to file");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while saving todos to file"));
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
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> LoadFromFile([FromQuery] string? filePath = null)
        {
            try
            {
                await _todoManager.LoadFromFileAsync(filePath);
                return Ok(ApiResponse<object>.SuccessResponse(null, "Todos loaded successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading todos from file");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while loading todos from file"));
            }
        }
    }
}