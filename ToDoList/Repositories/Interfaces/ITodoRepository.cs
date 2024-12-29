using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo> GetByIdAsync(int id);
        Task<Todo> CreateAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
    }
}
