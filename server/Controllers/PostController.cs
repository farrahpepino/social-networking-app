using server.Models;
using server.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]

    public class PostController: ControllerBase{
        
        private readonly PostService _postService;
        private readonly ILogger<PostController> _logger;
        
        public PostController(PostService postService, ILogger<PostController> logger){
            _postService = postService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostModel post){
            var createdPost = await _postService.CreatePost(post);

            if (createdPost == null)
                return StatusCode(500, "Failed to create post.");
            return Ok(createdPost); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(string id){
            var post = await _postService.GetPost(id);

            if(post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var success = await _postService.DeletePost(id);
            if (!success)
                return NotFound();

            return NoContent(); 
        }


    }
    
}