using server.Models;
using server.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]

    public class PostController: ControllerBase{
        
        private readonly PostService _postService;

        public PostController(PostService postService){
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostModel post){
            var createdPost = await _postService.CreatePost(post);

            if (createdPost == null)
                return StatusCode(500, "Failed to create post.");

            // return 201 Created + Location header + post in body
            return CreatedAtAction(
                nameof(GetPost),     
                new { id = createdPost.Id }, 
                createdPost              
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id){
            var post = await _postService.GetPost(id);

            if(post == null)
                return NotFound();
            return Ok(post);
        }

    }
    
}