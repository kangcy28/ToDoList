using Microsoft.AspNetCore.Mvc;
using ToDoList.DTOs;
using ToDoList.Services.Interfaces;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            var result = await _authService.RegisterAsync(registrationDto);

            if (result.Success)
                return Ok(new { Token = result.Token });

            return BadRequest(new { Errors = result.Errors });
        }

        /// <summary>
        /// Login with existing user credentials
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result.Success)
                return Ok(new { Token = result.Token });

            return BadRequest(new { Errors = result.Errors });
        }
    }
}