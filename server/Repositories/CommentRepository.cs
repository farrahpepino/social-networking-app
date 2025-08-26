using server.Data;
using server.Dtos;
using Dapper;

namespace server.Repositories{

    public class CommentRepository{
        private readonly DapperContext _context;

        public CommentRepository (DapperContext context){
            _context = context;
        }

        private const string InsertCommentQuery = @"INSERT INTO comments (AuthorId, Content, PostId, Id, CreatedAt) VALUES (@AuthorId, @Content, @PostId, @Id, @CreatedAt)";
        private const string SelectCommentsQuery = "SELECT comments.AuthorId, comments.content, comments.PostId, comments.CreatedAt, comments.Id, users.Username FROM comments JOIN users on comments.AuthorId = users.Id WHERE comments.PostId = @PostId ORDER BY comments.CreatedAt ASC";

        public async Task InsertComment(CommentDto comment){
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertCommentQuery, comment);
        }

        public async Task<IEnumerable<CommentDto>> GetComments (string postId){
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CommentDto>(SelectCommentsQuery, new { PostId = postId });
        }
    }



}