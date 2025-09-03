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
        private const string SelectInterestsQuery = "SELECT UserId1,users.Username, UserId2 FROM interests JOIN users ON UserId2=users.Id WHERE UserId1=@UserId";
        private const string DeleteInterestQuery = "DELETE FROM interests WHERE UserId1=@UserId1 and UserId2=@UserId2";

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

        public async Task<IEnumerable<InterestResponse>> GetInterests(string userId) {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<InterestResponse>(
                SelectInterestsQuery,
                new { UserId = userId }
            );
        }

        public async Task DeleteInterest(InterestDto interest){
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(DeleteInterestQuery, new{UserId1 = interest.UserId1, UserId2=interest.UserId2});
        }

    }
}