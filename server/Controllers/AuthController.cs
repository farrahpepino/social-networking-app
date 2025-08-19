using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationModel newUser)
        {
            var token = await _authService.RegisterUser(newUser);
            if (token == null)
                return BadRequest("Registration failed.");

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel user)
        {
            var token = await _authService.LoginUser(user);
            if (token == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new { token });
        }
    }
}
