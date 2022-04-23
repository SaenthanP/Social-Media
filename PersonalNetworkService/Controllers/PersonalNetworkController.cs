using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalNetworkService.MessageServicePublisher;
using PersonalNetworkService.Models;
using PersonalNetworkService.Services;

namespace PersonalNetworkService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalNetworkController: ControllerBase{
        private readonly IUserNetworkService _userNetworkService;
        private readonly IMessageClient _messageClient;

        public PersonalNetworkController(IUserNetworkService userNetworkService, IMessageClient messageClient)
        {
            _userNetworkService=userNetworkService;
            _messageClient=messageClient;
        }
        
        [HttpPost("follow/{userToFollowId}")]
        public async Task<ActionResult> FollowUser(string userToFollowId,[FromHeader(Name = "id")] string userId){
            var isFollowing=await _userNetworkService.IsFollowingUser(userToFollowId,userId);
            
            if(isFollowing){
                return Conflict("Already following user");
            }

            await _userNetworkService.FollowUser(userToFollowId,userId);

            isFollowing=await _userNetworkService.IsFollowingUser(userToFollowId,userId);
            if(isFollowing){
                var userToFollow=await _userNetworkService.GetUser(userToFollowId);
                var originalUser=await _userNetworkService.GetUser(userId);

                var publishFeedModel=new PublishNetworkModel();
                publishFeedModel.UserId=originalUser.UserId;
                publishFeedModel.Username=originalUser.Username;

                publishFeedModel.UserToInteractWithId=userToFollow.UserId;
                publishFeedModel.UserToInteractWithName=userToFollow.Username;

                _messageClient.FollowUser(publishFeedModel);
                return Ok("Followed user");
            }

            return Problem("Was not able to follow user");
        } 

        [HttpPost("unfollow/{userToUnfollowId}")]
        public async Task<ActionResult> UnfollowUser(string userToUnfollowId,[FromHeader(Name = "id")] string userId){
            var isFollowing=await _userNetworkService.IsFollowingUser(userToUnfollowId,userId);

            if(!isFollowing){
                return Conflict("Already not following user");
            }

            await _userNetworkService.UnfollowUser(userToUnfollowId,userId);

            isFollowing=await _userNetworkService.IsFollowingUser(userToUnfollowId,userId);
            if(!isFollowing){
                var userToUnfollow=await _userNetworkService.GetUser(userToUnfollowId);
                var originalUser=await _userNetworkService.GetUser(userId);

                var publishFeedModel=new PublishNetworkModel();
                publishFeedModel.UserId=originalUser.UserId;
                publishFeedModel.Username=originalUser.Username;

                publishFeedModel.UserToInteractWithId=userToUnfollow.UserId;
                publishFeedModel.UserToInteractWithName=userToUnfollow.Username;

                _messageClient.UnfollowUser(publishFeedModel);
                
                return Ok("UnFollowed user");
            }

            return Problem("Was not able to Unfollow user");
        } 

        [HttpGet("isfollowing/{userToCheckId}")]
        public async Task<ActionResult<bool>> IsFollowingUser(string userToCheckId,[FromHeader(Name = "id")] string userId){
            var isFollowing=await _userNetworkService.IsFollowingUser(userToCheckId,userId);

            return isFollowing;
        } 

    }
}