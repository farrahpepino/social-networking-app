//delete interest
//create interest

using server.Data;
using server.Models;
using server.Repositories;
using Dapper; 
using Microsoft.Extensions.Logging;


namespace server.Services{

    public class InterestService{

        private readonly ILogger<InterestService> _logger;
        private readonly InterestRepository _interestRepository;
    
        public InterestService(ILogger<InterestService> logger, InterestRepository interestRepository){
            _logger = logger;
            _interestRepository = interestRepository;
        }
    
        public async Task<Interest> CreateInterest(string userId1, string userId2){
            var interest = new Interest
            {
                Id = Guid.NewGuid().ToString(),
                UserId1 = userId1,
                UserId2 = userId2,
                CreatedAt = DateTime.Now
            };

            await _interestRepository.CreateInterest(interest); 
            return interest;
        }
    }
}