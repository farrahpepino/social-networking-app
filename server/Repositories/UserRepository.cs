using server.Data;
using server.Models;
using Dapper;

namespace server.Repositories{

    public class UserRepository{
        private readonly DapperContext _context;

        public UserRepository(DapperContext context){
            _context = context;    
        }

        private const string InsertUserQuery = @"INSERT INTO users (Id, Username, Email, HashedPassword, CreatedAt) 
                        VALUES (@Id, @Username, @Email, @HashedPassword, @CreatedAt)";
        private const string SelectUserByEmailQuery = "SELECT * FROM users WHERE Email=@Email";

        public async Task InsertUser(RegistrationModel newUser) {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(InsertUserQuery, newUser);
        }

        public async Task<RegistrationModel?> GetUserByEmail(string email) {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<RegistrationModel>(
                SelectUserByEmailQuery, new { Email = email });
        }

    }    
}