using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            todo.CreatedDate = DateTime.UtcNow;
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task UpdateAsync(Todo todo)
        {
            var existingTodo = await _context.Todos.FindAsync(todo.Id);
            if (existingTodo == null)
                throw new KeyNotFoundException($"Todo with ID {todo.Id} not found.");

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            todo.IsCompleted = !todo.IsCompleted;
            await _context.SaveChangesAsync();

            return todo.IsCompleted;
        }

    }
}
