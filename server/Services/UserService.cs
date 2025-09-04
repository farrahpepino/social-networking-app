using server.Repositories;
using server.Models;

namespace server.Services{
    public class UserService{
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository){
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserSearchResponse>> SearchUsers(string query){ 
            return await _userRepository.SearchUsers(query);
        }

        public async Task<User> GetUserInfo(string username){
            return await _userRepository.GetUserInfo(username);
        }

    }
}