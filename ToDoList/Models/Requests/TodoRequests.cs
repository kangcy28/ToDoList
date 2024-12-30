using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ToDoList.Models.Requests
{
    public class TodoCreateRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
        [SwaggerSchema(Description = "The title of the todo item")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [SwaggerSchema(Description = "The description of the todo item")]
        public string? Description { get; set; }
    }

    public class TodoUpdateRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
        [SwaggerSchema(Description = "The new title of the todo item")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [SwaggerSchema(Description = "The new description of the todo item")]
        public string? Description { get; set; }
    }
}