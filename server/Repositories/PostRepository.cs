using server.Data;
using server.Models;
using Dapper;

namespace server.Repositories{
    public class PostRepository{
        private readonly DapperContext _context;

        public PostRepository(DapperContext context){
            _context = context;
        }

        private const string InsertPostQuery = @"INSERT INTO posts (Id, AuthorId, Content, CreatedAt) VALUES (@Id, @AuthorId, @Content, @CreatedAt);";
        private const string DeletePostByIdQuery = "DELETE FROM posts WHERE Id = @Id";
        private const string SelectPostByIdQuery = "SELECT posts.Id, AuthorId, Content, posts.CreatedAt, users.Username FROM posts JOIN users on users.Id = posts.AuthorId WHERE Id = @Id ";
        private const string SelectPostsQuery = "SELECT posts.Id, AuthorId, Content, posts.CreatedAt, users.Username FROM posts JOIN users on users.Id = posts.AuthorId";
        
        public async Task InsertPost(PostModel post) {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertPostQuery, post);
        }

        public async Task<int> DeletePost (string postId){
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(DeletePostByIdQuery, new {Id = postId});
             
        }

        public async Task<PostModel?> GetPostById(string postId){
            
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<PostModel>(SelectPostByIdQuery, new { Id = postId });
        
        }

        public async Task<IEnumerable<PostModel>> GetPosts()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PostModel>(SelectPostsQuery);
        }

    }
}