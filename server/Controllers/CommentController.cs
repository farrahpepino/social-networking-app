using server.Dtos;
using server.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    [Authorize] 

    public class CommentController: ControllerBase{

        private readonly CommentService _commentService;

        public CommentController (CommentService commentService){
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment ([FromBody] CommentDto comment){
            var createdComment = await _commentService.CreateComment(comment);
            if (createdComment == null)
                return StatusCode(500, "Failed to create a comment.");
            return Ok(createdComment); 
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetComments (string postId){
            var comments = await _commentService.GetComments(postId);
            if (comments == null){
                return NotFound();
            }
            return Ok(comments);
        }        
    }
}