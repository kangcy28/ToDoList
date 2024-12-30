using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class UpdateTodoDto
    {
        [Required]
        [SwaggerSchema(Description = "The unique identifier for the todo item")]
        public int Id { get; set; }

        [Required]
        [SwaggerSchema(Description = "The title of the todo item")]
        public string Title { get; set; }

        [SwaggerSchema(Description = "The description of the todo item")]
        public string Description { get; set; }

        [SwaggerSchema(Description = "Indicates whether the todo item is completed")]
        public bool IsCompleted { get; set; }
    }
}
