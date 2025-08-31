using server.Repositories;
using server.Models;
using Microsoft.Extensions.Logging;

namespace server.Services{
    public class UserService{
        private readonly UserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(UserRepository userRepository, ILogger<UserService> logger){
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<UserSearchResult>> SearchUsers(string query){
            try{ 
                return await _userRepository.SearchUsers(query);
            }
            catch (Exception ex){
                _logger.LogError(ex, "Error searching for users");
                throw;
            }
        }

    }
}