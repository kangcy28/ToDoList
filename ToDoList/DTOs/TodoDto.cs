using Swashbuckle.AspNetCore.Annotations;

namespace ToDoList.DTOs
{
    public class TodoDto
    {
        [SwaggerSchema(Description = "The unique identifier for the todo item")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "The title of the todo item")]
        public string Title { get; set; }

        [SwaggerSchema(Description = "The description of the todo item")]
        public string Description { get; set; }

        [SwaggerSchema(Description = "Indicates whether the todo item is completed")]
        public bool IsCompleted { get; set; }

        [SwaggerSchema(Description = "The date and time when the todo item was created")]
        public DateTime CreatedDate { get; set; }
    }
}
