using ToDoList.DTOs;
using ToDoList.Models;
namespace ToDoList.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserRegistrationDto model);
        Task<AuthResult> LoginAsync(UserLoginDto model);
    }
}
