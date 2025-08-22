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
                return null;
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
                return null;
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

        public async Task<IEnumerable<LikesModel>> GetLikesByPostId(string postId)
        {
            try
            {
                return await _postRepository.GetLikesByPostId(postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching the like from post");
                return Enumerable.Empty<PostModel>();
            }
        }

        public async Task<LikeModel?> LikePost(LikeModel like){
            try
            {
                like.Id = Guid.NewGuid().ToString();
                like.CreatedAt = DateTime.Now;
                return await _postRepository.LikePost(like);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking the post");
                return null;
            }
        }

        public async Task<bool> UnlikePost(LikeModel like){
            try{
                var affectedRows =  await _postRepository.UnlikePost(like.PostId, like.Id);
            
                if (affectedRows > 0){
                    return true;
                }

                else{
                    _logger.LogWarning("No post found with that Id.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unliking the post");
                return false.
            }
        }
    
    }
}
