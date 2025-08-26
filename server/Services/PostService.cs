using server.Data;
using server.Models;
using server.Repositories;
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

        private readonly ILogger<PostService> _logger;
        private readonly PostRepository _postRepository;
    
        public PostService(ILogger<PostService> logger, PostRepository postRepository){
            _logger = logger;
            _postRepository = postRepository;
        }
    
        public async Task<PostModel?> CreatePost(PostModel post){
            try{
        
            post.Id = Guid.NewGuid().ToString();
            post.CreatedAt = DateTime.Now;
        
            await _postRepository.InsertPost(post);
            return post;
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error creating post");
                throw;
            }
        }

        public async Task<bool> DeletePost(string postId){
           try{

            var affectedRows = await _postRepository.DeletePost(postId);

            if (affectedRows > 0){
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
                return await _postRepository.GetPostById(postId);
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error fetching post");
                throw;
            }
        }

        public async Task<IEnumerable<PostModel>> GetPostsByUserId(string authorId)
        {
            try
            {
                return await _postRepository.GetPostsByUserId(authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts");
                return Enumerable.Empty<PostModel>();
            }
        }
        
        public async Task<IEnumerable<PostModel>> GetPosts()
        {
            try
            {
                return await _postRepository.GetPosts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts");
                return Enumerable.Empty<PostModel>();
            }
        }
        
        public async Task<bool> LikeExists(string PostId, string LikerId)
        {
            try
            {
                return await _postRepository.LikeExists(PostId, LikerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if like exists");
                return false;
            }
        }


        public async Task<IEnumerable<LikeModel>> GetLikesByPostId(string postId)
        {
            try
            {
                return await _postRepository.GetLikesByPostId(postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching the like from post");
                return Enumerable.Empty<LikeModel>();
            }
        }

        public async Task<LikeModel> LikePost(LikeModel like){
            try
            {
                like.Id = Guid.NewGuid().ToString();
                like.CreatedAt = DateTime.Now;
                await _postRepository.LikePost(like);
                return like;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking the post");
                throw;
            }
        }

        public async Task<bool> UnlikePost(string PostId, string LikerId){
            try{
                var affectedRows =  await _postRepository.UnlikePost(PostId, LikerId);
                return (affectedRows > 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unliking the post");
                return false;
            }
        }
    
    }
}
