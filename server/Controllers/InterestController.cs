using server.Services;
using server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers{

    [ApiController]
    [Route("[controller]")]
    [Authorize] 
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

        [HttpGet]
        public async Task<IActionResult> GetInterests(string userId){
            var interests = await _interestService.GetInterests(userId);
            return Ok(interests); 
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInterest(InterestDto interest){
            await _interestService.DeleteInterest(interest);
            return Ok();
        }

    }
}