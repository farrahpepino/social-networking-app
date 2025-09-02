using server.Services;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase{
        private readonly UserService _userService;

        public UserController(UserService userService){
            _userService = userService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty");
            var users = await _userService.SearchUsers(query); 
            if (users.Any())
                return Ok(users);  
            else
                return NotFound();
        }



    }
}