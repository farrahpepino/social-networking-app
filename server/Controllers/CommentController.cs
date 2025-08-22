using server.Models;
using server.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateComment ([FromBody] CommentModel comment){
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