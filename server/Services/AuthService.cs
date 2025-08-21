using server.Data;
using server.Models;
using server.Repositories;
using Microsoft.Extensions.Logging;
using BCrypt.Net; // for hashing password and verification

namespace server.Services{
    public class AuthService{
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserRepository _userRepository;


        public AuthService (ILogger<AuthService> logger, IJwtService jwtService, UserRepository userRepository ){
            _logger = logger;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<string?> RegisterUser(RegistrationModel newUser){
            if(newUser.HashedPassword == "" || newUser.Username == "" || newUser.Email == "" ){
                _logger.LogWarning($"Some required fields are missing or invalid.");
                return null;
            }
            newUser.Id = Guid.NewGuid().ToString();
            newUser.CreatedAt = DateTime.Now;
            newUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.HashedPassword);
                
            await _userRepository.InsertUser(newUser);
            var token = _jwtService.GenerateToken(userId: newUser.Id, username: newUser.Username, email: newUser.Email);
            return token;
        }   

        public async Task<string?> LoginUser(LoginModel user)
        {
           
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser == null)
            {
                 _logger.LogWarning($"No user found.");
                return null; 
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.HashedPassword);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Invalid password.");
                return null; 
            }

            var token = _jwtService.GenerateToken(userId: existingUser.Id, username: existingUser.Username, email: existingUser.Email);
            return token;
        }

    }
}