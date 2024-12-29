using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    // Models/Todo.cs
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ValidationException("Title cannot be empty.");

            if (Title.Length > 100)
                throw new ValidationException("Title cannot be longer than 100 characters.");

            if (Description?.Length > 500)
                throw new ValidationException("Description cannot be longer than 500 characters.");
        }
    }
}
