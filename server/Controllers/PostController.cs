using server.Models;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    [Authorize] 

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
            return Ok(createdPost); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(string id){
            var post = await _postService.GetPost(id);
            if(post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts(){
            var posts = await _postService.GetPosts();
            if(posts == null)
                return NotFound();
            return Ok(posts);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id){
            var success = await _postService.DeletePost(id);
            if (!success)
                return NotFound();
            return NoContent(); 
        }

        [HttpDelete("unlike-post")]
        public async Task<IActionResult> UnlikePost([FromBody] LikeModel like){
            var success = await _postService.UnlikePost(like.PostId, like.Id);
            if (!success)
                return NotFound();
            return NoContent(); 
        }

        [HttpPost("like-post")]
        public async Task<IActionResult> LikePost([FromBody] LikeModel like){
            var liker = await _postService.LikePost(like);
            if (liker == null)
                return StatusCode(500, "Failed to like post.");
            return Ok(liker);  
        }

        [HttpGet("{postId}/likes")]
        public async Task<IActionResult> GetLikesByPostId(string postId){
            var likes = await _postService.GetLikesByPostId(postId);
            if(likes == null)
                return NotFound();
            return Ok(posts);
        }



    }
}