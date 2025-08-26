using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Services;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase{
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto newUser){
            try {
                var token = await _authService.RegisterUser(newUser);
                if (token!=null){
                    return Ok(new { token });
                }
                return BadRequest();
            }
            catch (Exception ex) {
                return BadRequest("Registration failed: " + ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto user){
            try{
                var token = await _authService.LoginUser(user);
                if (token == null)
                    return Unauthorized("Invalid email or password.");
                return Ok(new { token });
            }
            catch (Exception ex){
                return BadRequest("Login failed: " + ex.Message);
            }
        }   
    }
}
