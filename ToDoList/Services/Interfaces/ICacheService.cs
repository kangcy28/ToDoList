using ToDoList.DTOs;

namespace ToDoList.Services.Interfaces
{
    public interface ICacheService
    {
        Task<TodoDto?> GetTodoAsync(string key);
        Task<IEnumerable<TodoDto>?> GetTodoListAsync(string key);
        Task SetTodoAsync(string key, TodoDto value, TimeSpan? expirationTime = null);
        Task SetTodoListAsync(string key, IEnumerable<TodoDto> value, TimeSpan? expirationTime = null);
        Task RemoveAsync(string key);
    }
}
