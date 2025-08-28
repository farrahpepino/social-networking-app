using server.Data;
using server.Models;
using server.Repositories;
using Dapper;
using Microsoft.Extensions.Logging;

namespace server.Services{
    public class CommentService{
        private readonly DapperContext _context;
        private readonly ILogger<CommentService> _logger;
        private readonly CommentRepository _commentRepository;

        public CommentService(DapperContext context, ILogger<CommentService> logger, CommentRepository commentRepository){
                _context = context;
                _logger = logger;
                _commentRepository = commentRepository;
        }

        public async Task<Comment?> CreateComment(Comment comment){
                try{
                        comment.Id = Guid.NewGuid().ToString();
                        comment.CreatedAt = DateTime.Now;
                        await _commentRepository.InsertComment(comment);
                        return comment;
                }
                catch (Exception ex){
                        _logger.LogError($"Error creating comment: {ex.Message}");
                        throw;
                }
        }

        public async Task<IEnumerable<Comment>> GetComments(string postId){
                try{
                        return await _commentRepository.GetComments(postId);     
                }
                catch (Exception ex){
                        _logger.LogError(ex, $"Error fetching comments");
                        return Enumerable.Empty<Comment>();
                }
        }
   
    }
}