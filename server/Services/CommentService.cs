using server.Data;
using server.Models;
using server.Repositories;
using Dapper;

namespace server.Services{
    public class CommentService{
        private readonly DapperContext _context;
        private readonly CommentRepository _commentRepository;

        public CommentService(DapperContext context CommentRepository commentRepository){
                _context = context;
                _commentRepository = commentRepository;
        }

        public async Task<Comment?> CreateComment(Comment comment){
                comment.Id = Guid.NewGuid().ToString();
                comment.CreatedAt = DateTime.Now;
                await _commentRepository.InsertComment(comment);
                return comment;
            
        }

        public async Task<IEnumerable<Comment>> GetComments(string postId){
                return await _commentRepository.GetComments(postId);     
        }
   
    }
}