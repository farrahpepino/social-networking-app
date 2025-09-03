using server.Services;
using server.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class InterestController: ControllerBase{

        private readonly InterestService _interestService;

        public InterestController(InterestService interestService){
            _interestService=interestService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateInterest(string userId1, string userId2){
            var createdInterest = await _interestService.CreateInterest(userId1, userId2);
            return Ok(createdInterest); 
        }

    }
}