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
    
        public async Task<Post?> CreatePost(Post post){
            post.ImageUrl?.ToString();
            post.Id = Guid.NewGuid().ToString();
            post.CreatedAt = DateTime.Now;
            await _postRepository.InsertPost(post);
            
            return post;
        }

        public async Task<bool> DeletePost(string postId){
            var affectedRows = await _postRepository.DeletePost(postId);

            if (affectedRows > 0){
                return true;
            }

            else{
                _logger.LogWarning("No post found with that Id.");
                return false;
            }

        }

        public async Task<Post?> GetPost(string postId){

            return await _postRepository.GetPostById(postId);
            
        }

        public async Task<IEnumerable<Post>> GetPostsByUserId(string authorId)
        {
            return await _postRepository.GetPostsByUserId(authorId);
        }
        
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postRepository.GetPosts();
        }
        
        public async Task<bool> LikeExists(string PostId, string LikerId)
        {
            return await _postRepository.LikeExists(PostId, LikerId);
        }


        public async Task<IEnumerable<Like>> GetLikesByPostId(string postId)
        {
            return await _postRepository.GetLikesByPostId(postId);
        }

        public async Task<Like> LikePost(Like like){
            like.Id = Guid.NewGuid().ToString();
            like.CreatedAt = DateTime.Now;
            await _postRepository.LikePost(like);
            return like;
            
        }

        public async Task<bool> UnlikePost(string PostId, string LikerId){
            var affectedRows =  await _postRepository.UnlikePost(PostId, LikerId);
            return (affectedRows > 0);
        }
    
    }
}
