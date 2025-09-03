using server.Data;
using server.Models;
using Dapper;

namespace server.Repositories{
    public class InterestRepository{
        private readonly DapperContext _context;

        public InterestRepository(DapperContext context){
            _context = context;
        }

        private const string InsertInterestQuery = "CALL CreateInterest(@p_userId1, @p_userId2, @p_interestId, @p_createdAt)";

        public async Task CreateInterest(Interest interest) {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                InsertInterestQuery,
                new
                {
                    p_userId1 = interest.UserId1,
                    p_userId2 = interest.UserId2,
                    p_interestId = interest.Id,
                    p_createdAt = interest.CreatedAt
                }
            );
        }
    }
}