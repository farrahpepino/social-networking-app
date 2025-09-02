using server.Repositories;
using server.Models;

namespace server.Services{
    public class UserService{
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository){
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserSearchResult>> SearchUsers(string query){ 
            return await _userRepository.SearchUsers(query);
        }

    }
}