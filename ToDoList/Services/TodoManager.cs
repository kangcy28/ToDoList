using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Services
{
    public class TodoManager
    {
        private readonly ITodoRepository _todoRepository;
        private readonly FileService _fileService;

        public TodoManager(ITodoRepository todoRepository, FileService fileService)
        {
            _todoRepository = todoRepository;
            _fileService = fileService;
        }

        public async Task<Todo> AddTodoAsync(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            var todo = new Todo
            {
                Title = title,
                Description = description ?? "",
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
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");
            return todo;
        }

        public async Task UpdateTodoAsync(int id, string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            todo.Title = title;
            todo.Description = description ?? "";
            await _todoRepository.UpdateAsync(todo);
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            await _todoRepository.DeleteAsync(id);
        }

        public async Task<bool> ToggleTodoStatusAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            return await _todoRepository.ToggleStatusAsync(id);
        }

        public async Task SaveToFileAsync(string filePath)
        {
            var todos = await _todoRepository.GetAllAsync();
            if (!todos.Any())
                throw new InvalidOperationException("No todos to save.");

            await _fileService.SaveTodosAsync(todos, filePath);
        }

        public async Task LoadFromFileAsync(string filePath)
        {
            
            var todos = await _fileService.LoadTodosAsync(filePath);
            if (!todos.Any())
                throw new InvalidOperationException("No todos found in file.");

            // Clear existing todos before loading new ones
            var existingTodos = await _todoRepository.GetAllAsync();
            foreach (var todo in existingTodos)
            {
                await _todoRepository.DeleteAsync(todo.Id);
            }

            if (!todos.Any())
            {
                Console.WriteLine("No todos found in file. Database cleared.");
                return;
            }

            // Add new todos
            foreach (var todo in todos)
            {
                // Reset ID and CreatedDate for new entries
                todo.Id = 0;
                if (todo.CreatedDate == default)
                    todo.CreatedDate = DateTime.UtcNow;

                await _todoRepository.CreateAsync(todo);
            }
        }
    }
}