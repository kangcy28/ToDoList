using AutoMapper;
using ToDoList.DTOs;
using ToDoList.Exceptions;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services
{
    public class TodoManager
    {
        private readonly ITodoRepository _todoRepository;
        private readonly FileService _fileService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private const string TodoListCacheKey = "TodoList_All";
        private const string TodoItemKeyPrefix = "Todo_";

        public TodoManager(ITodoRepository todoRepository, FileService fileService, ICacheService cacheService,
        IMapper mapper)
        {
            _todoRepository = todoRepository;
            _fileService = fileService;
            _cacheService = cacheService;
            _mapper = mapper;
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

        public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
        {
            var cachedTodos = await _cacheService.GetTodoListAsync(TodoListCacheKey);
            if (cachedTodos != null)
            {
                return cachedTodos;
            }

            // If not in cache, get from repository
            var todos = await _todoRepository.GetAllAsync();
            var todoDtos = _mapper.Map<IEnumerable<TodoDto>>(todos);

            // Cache the results
            await _cacheService.SetTodoListAsync(TodoListCacheKey, todoDtos);

            return todoDtos;
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            var cacheKey = $"{TodoItemKeyPrefix}{id}";

            // Try to get from cache first
            var cachedTodo = await _cacheService.GetTodoAsync(cacheKey);
            if (cachedTodo != null)
            {
                return cachedTodo;
            }

            // If not in cache, get from repository
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                throw new TodoNotFoundException(id);
            }

            var todoDto = _mapper.Map<TodoDto>(todo);

            // Cache the result
            await _cacheService.SetTodoAsync(cacheKey, todoDto);

            return todoDto;
        }

        public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                throw new KeyNotFoundException($"Todo with ID {id} not found.");
            }

            _mapper.Map(updateTodoDto, todo);
            await _todoRepository.UpdateAsync(todo);

            var todoDto = _mapper.Map<TodoDto>(todo);

            // Invalidate both caches
            await _cacheService.RemoveAsync($"Todo_{id}");
            await _cacheService.RemoveAsync("TodoList_All");

            return todoDto;
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                throw new KeyNotFoundException($"Todo with ID {id} not found.");

            await _todoRepository.DeleteAsync(id);

            // Invalidate both the single item cache and the list cache
            await _cacheService.RemoveAsync($"Todo_{id}");
            await _cacheService.RemoveAsync("TodoList_All");
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