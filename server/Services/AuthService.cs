using server.Data;
using server.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace server.Services{
    public class AuthService{
        private readonly DapperContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;

        private const string InsertUserQuery = @"INSERT INTO users (Id, Username, Email, HashedPassword, CreatedAt) 
                        VALUES (@Id, @Username, @Email, @HashedPassword, @CreatedAt)";
        private const string SelectUserByEmailQuery = "SELECT * FROM users WHERE Email=@Email";

        public AuthService (DapperContext context, ILogger<AuthService> logger, IJwtService jwtService){
            _context = context;
            _logger = logger;
            _jwtService = jwtService;

        }

        public async Task<string?> RegisterUser(RegistrationModel newUser){
            if(newUser.HashedPassword == "" || newUser.Username == "" || newUser.Email == "" ){
                _logger.LogWarning($"Some required fields are missing or invalid.");
                return null;
            }
            newUser.Id = Guid.NewGuid().ToString();
            newUser.CreatedAt = DateTime.Now;
            newUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.HashedPassword);
                
            
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertUserQuery, newUser);

            _logger.LogInformation($"User registered.");
            var token = _jwtService.GenerateToken(userId: newUser.Id, username: newUser.Username, email: newUser.Email);
            return token;
        }   

        public async Task<string?> LoginUser(LoginModel user)
        {
            using var connection = _context.CreateConnection();
            var existingUser = await connection.QuerySingleOrDefaultAsync<RegistrationModel>(SelectUserByEmailQuery, new { user.Email });

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

            _logger.LogInformation($"User logged in.");
            var token = _jwtService.GenerateToken(userId: existingUser.Id, username: existingUser.Username, email: existingUser.Email);
            return token;
            
        }

    }
}