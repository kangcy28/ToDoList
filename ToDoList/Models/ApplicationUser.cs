using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for user's todos
        public ICollection<Todo> Todos { get; set; }
    }
}