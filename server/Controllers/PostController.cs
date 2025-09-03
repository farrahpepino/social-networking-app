using server.Models;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> CreatePost([FromBody] Post post){
            var createdPost = await _postService.CreatePost(post);
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

        [HttpGet("{authorId}/posts")]
        public async Task<IActionResult> GetPostsByUserId(string authorId){
            var posts = await _postService.GetPostsByUserId(authorId);
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

        [HttpDelete("{PostId}/unlike-post/{LikerId}")]
        public async Task<IActionResult> UnlikePost(string PostId, string LikerId){
            var success = await _postService.UnlikePost(PostId, LikerId);
            if (!success)
                return NotFound();
            return Ok(); 
        }

        [HttpPost("like-post")]
        public async Task<IActionResult> LikePost([FromBody] Like like){
            var liked = await _postService.LikePost(like);
            return Ok(liked);  
        }

        [HttpGet("{postId}/liked-by/{userId}")]
        public async Task<IActionResult> LikeExists(string postId, string userId)
        {
            var exists = await _postService.LikeExists(postId, userId);
            return Ok(exists); 
        }

        [HttpGet("{postId}/likes")]
        public async Task<IActionResult> GetLikesByPostId(string postId){
            var likes = await _postService.GetLikesByPostId(postId);
            if(likes == null)
                return NotFound();
            return Ok(likes);
        }
    }
}