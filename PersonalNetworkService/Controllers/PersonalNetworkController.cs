using System;
using System.Threading.Tasks;
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
        
        [HttpPost("follow/{userToFollowId}")]
        public async Task<ActionResult> FollowUser(string userToFollowId,[FromHeader(Name = "id")] string userId){
            await _userNetworkService.FollowUser(userToFollowId,userId);
            return Ok("followed");
        } 

        [HttpPost("unfollow/{userToUnfollowId}")]
        public async Task<ActionResult> UnfollowUser(string userToUnfollowId,[FromHeader(Name = "id")] string userId){
            await _userNetworkService.UnfollowUser(userToUnfollowId,userId);
            return Ok("unfollowed");
        } 

        [HttpGet("isfollowing/{userToCheck}")]
        public async Task<ActionResult<bool>> IsFollowingUser(string userToCheck,[FromHeader(Name = "id")] string userId){
            var isFollowing=await _userNetworkService.IsFollowingUser(userToCheck,userId);

            return isFollowing;
        } 

    }
}