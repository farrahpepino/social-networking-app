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
        private const string SelectPostByIdQuery = "SELECT posts.Id, AuthorId, Content, posts.CreatedAt, users.Username FROM posts JOIN users on users.Id = posts.AuthorId WHERE posts.Id = @Id ";
        private const string SelectPostsByUserIdQuery = "SELECT posts.Id, AuthorId, Content, posts.CreatedAt, users.Username FROM posts JOIN users on users.Id = posts.AuthorId WHERE AuthorId = @AuthorId ORDER BY posts.CreatedAt DESC";
        private const string SelectPostsQuery = "SELECT posts.Id, AuthorId, Content, posts.CreatedAt, users.Username FROM posts JOIN users on users.Id = posts.AuthorId ORDER BY posts.CreatedAt DESC";
        private const string InsertLikeQuery = @"INSERT IGNORE INTO likes (Id, PostId, LikerId, CreatedAt) VALUES (@Id, @PostId, @LikerId, @CreatedAt);";
        private const string DeleteLikeQuery = "DELETE FROM likes WHERE PostId = @PostId AND LikerId=@LikerId";
        private const string SelectLikesByPostIdQuery = "SELECT * FROM likes JOIN users on users.Id = likes.likerId WHERE PostId=@PostId ORDER BY likes.CreatedAt ASC";

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

        public async Task<IEnumerable<PostModel>> GetPostsByUserId(string authorId){
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PostModel>(SelectPostsByUserIdQuery, new { AuthorId = authorId });
        }

        public async Task<IEnumerable<PostModel>> GetPosts(){
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PostModel>(SelectPostsQuery);
        }

        public async Task LikePost(LikeModel like){
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertLikeQuery, like);
        }

        public async Task<int> UnlikePost(string PostId, string LikerId){
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(DeleteLikeQuery, new {PostId = PostId, LikerId = LikerId});
        }
        
        public async Task<IEnumerable<LikeModel>> GetLikesByPostId(string PostId){
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<LikeModel>(SelectLikesByPostIdQuery, new { PostId = PostId });
        }


    }
}