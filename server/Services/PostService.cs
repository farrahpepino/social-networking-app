using server.Data;
using server.Models;
using Dapper; 
using Microsoft.Extensions.Logging;

/*
I've learned using just one connection for all methods is 
not thread-safe. If API handles multiple requests at the same time, it
can cause errors.

Additionally, you must open and close it properly. It shouldn't be 
open for the entire app lifetime.
*/

namespace server.Services{

    public class PostService{

        private readonly DapperContext _context;
        private readonly ILogger<PostService> _logger;

        private const string InsertPostQuery = @"INSERT INTO posts (Id, AuthorId, Content, CreatedAt) VALUES (@Id, @AuthorId, @Content, @CreatedAt);";
        private const string DeletePostByIdQuery = "DELETE FROM posts WHERE Id = @Id";
        private const string SelectPostByIdQuery = "SELECT * FROM posts WHERE Id = @Id";

        public PostService(DapperContext context, ILogger<PostService> logger){
            _context = context;
            _logger = logger;
        }
    

        public async Task<PostModel?> CreatePost(PostModel post){

            try{
        
            post.Id = Guid.NewGuid().ToString();
            post.CreatedAt = DateTime.Now;
            
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertPostQuery, post);
            _logger.LogInformation("Post created!");
            
            return post;
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error creating post");
                return null;
            }
           
        }

        public async Task<bool> DeletePost(string postId){
           try{

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(DeletePostByIdQuery, new {Id = postId});

            if (affectedRows > 0){
                _logger.LogInformation("Post deleted!");
                return true;
            }

            else{
                _logger.LogWarning("No post found with that Id.");
                return false;
            }
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error deleting post");
                return false;
            }

        }

        public async Task<PostModel?> GetPost(string postId){
            try{

            using var connection = _context.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<PostModel>(SelectPostByIdQuery, new { Id = postId });
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error getting post");
                return null;
            }
        }
    
    }
}