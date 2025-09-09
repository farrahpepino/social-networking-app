using server.Data;
using server.Models;
using server.Repositories;
using Dapper; 
using Microsoft.Extensions.Logging;
using AwsS3.Models;
using AwsS3.Services;

namespace server.Services{

    public class PostService{

        private readonly ILogger<PostService> _logger;
        private readonly PostRepository _postRepository;
        private readonly IConfiguration _config;
         private readonly IStorageService _storageService;

    
        public PostService(IConfiguration config, ILogger<PostService> logger, PostRepository postRepository, IStorageService storageService){
            _logger = logger;
            _postRepository = postRepository;
            _storageService = storageService;
            _config = config;
        }
    
        public async Task<Post?> CreatePost(Post post){
            post.ImageUrl?.ToString();
            post.ImageKey?.ToString();
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

        public async Task<IEnumerable<Post>> GetFeedPosts(string userId1)
        {
            return await _postRepository.GetFeedPosts(userId1);
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
