using Microsoft.AspNetCore.Mvc;
using PersonalNetworkService.Services;

namespace PersonalNetworkService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalNetworkController: ControllerBase{
        private readonly IUserNetworkService _userNetworkService;

        public PersonalNetworkController(IUserNetworkService userNetworkService)
        {
            _userNetworkService=userNetworkService;
        }
        
        [HttpPost("{userid}")]
        public ActionResult FollowUser(string userid){
            _userNetworkService.FollowUser(userid);
            return Ok("yes");
        } 


    }
}