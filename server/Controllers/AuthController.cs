using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;
using Microsoft.Extensions.Logging;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    
    public class AuthController: ControllerBase{
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Registration newUser){
            var token = await _authService.RegisterUser(newUser);
            if (token!=null){
                return Ok(new { token });
            }
                return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Login user){
            var token = await _authService.LoginUser(user);
            if (token == null)
                return Unauthorized("Invalid email or password.");
            return Ok(new { token });
        }   
    }
}
