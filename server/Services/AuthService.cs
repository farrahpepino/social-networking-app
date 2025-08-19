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


        
        public AuthService (DapperContext context, ILogger<AuthService> logger, IJwtService jwtService){
            _context = context;
            _logger = logger;
            _jwtService = jwtService;

        }


        public async Task<string?> RegisterUser(RegistrationModel newUser){
            try{
                newUser.Id = Guid.NewGuid().ToString();
                newUser.CreatedAt = DateTime.Now;
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);


                var query = @"INSERT INTO users (Username, Email, HashedPassword, Id, CreatedAt) VALUES (@Username, @Email, @Password, @Id, @CreatedAt)";
                using var connection = _context.CreateConnection();
                await connection.ExecuteAsync(query, newUser);

                _logger.LogInformation($"User registered!");
                var token = _jwtService.GenerateToken(userId: newUser.Id, username: newUser.Username, email: newUser.Email);
                return token;

            }
            catch (Exception ex){
                _logger.LogError($"Error registering user: {ex.Message}");
                return null;
            }
        }   

        public async Task<string?> LoginUser(LoginModel user)
        {
            try
            {
                var query = "SELECT * FROM users WHERE Email=@Email";
                using var connection = _context.CreateConnection();
                var existingUser = await connection.QuerySingleOrDefaultAsync<RegistrationModel>(query, new { user.Email });

                if (existingUser == null)
                {
                    return null; 
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);
                if (!isPasswordValid)
                {
                    return null; 
                }

                
                var token = _jwtService.GenerateToken(userId: existingUser.Id, username: existingUser.Username, email: existingUser.Email);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging in user: {ex.Message}");
                return null;
            }
        }





    }

























}