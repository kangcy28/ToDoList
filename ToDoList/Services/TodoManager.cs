using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Services
{
    public class TodoManager
    {
        private readonly ITodoRepository _todoRepository;

        public TodoManager(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Todo> AddTodoAsync(string title, string description)
        {
            var todo = new Todo
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedDate = DateTime.UtcNow
            };

            return await _todoRepository.CreateAsync(todo);
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _todoRepository.GetAllAsync();
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _todoRepository.GetByIdAsync(id);
        }

        public async Task UpdateTodoAsync(int id, string title, string description)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            todo.Title = title;
            todo.Description = description;

            await _todoRepository.UpdateAsync(todo);
        }

        public async Task DeleteTodoAsync(int id)
        {
            await _todoRepository.DeleteAsync(id);
        }

        public async Task<bool> ToggleTodoStatusAsync(int id)
        {
            return await _todoRepository.ToggleStatusAsync(id);
        }
    }
}
