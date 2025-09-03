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
        public async Task<IActionResult> CreateInterest(InterestDto interest){
            var createdInterest = await _interestService.CreateInterest(interest);
            return Ok(createdInterest); 
        }

    }
}