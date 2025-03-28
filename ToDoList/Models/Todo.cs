﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    /// <summary>
    /// Represents a todo item
    /// </summary>
    [SwaggerSchema(Description = "Represents a todo item in the system")]
    public class Todo
    {
        /// <summary>
        /// The unique identifier for the todo item
        /// </summary>
        [SwaggerSchema(Description = "The unique identifier for the todo item")]
        public int Id { get; set; }

        /// <summary>
        /// The title of the todo item
        /// </summary>
        [Required]
        [SwaggerSchema(Description = "The title of the todo item")]
        public string Title { get; set; }

        /// <summary>
        /// The description of the todo item
        /// </summary>
        [SwaggerSchema(Description = "The description of the todo item")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the todo item is completed
        /// </summary>
        [SwaggerSchema(Description = "Indicates whether the todo item is completed")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// The date and time when the todo item was created
        /// </summary>
        [SwaggerSchema(Description = "The date and time when the todo item was created")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The identifier of the user who owns this todo item
        /// </summary>
        [Required(ErrorMessage = "User ID is required")]
        [SwaggerSchema(Description = "The unique identifier of the user who owns this todo item")]
        public string? UserId { get; set; }

        /// <summary>
        /// Navigation property for the user who owns this todo item
        /// </summary>
        [SwaggerSchema(Description = "The associated user object")]
        [ForeignKey("UserId")]  // Explicit foreign key relationship
        public virtual ApplicationUser? User { get; set; }
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
