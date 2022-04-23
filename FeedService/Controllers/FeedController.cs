using System.Collections.Generic;
using FeedService.Models;
using FeedService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase {
        private readonly IPostService _postService;

        public FeedController(IPostService postservice)
        {
            _postService=postservice;
        }

        [HttpGet("homefeed")]
        public ActionResult<IEnumerable<PublishedPostModel>> GetHomeFeed([FromHeader(Name = "id")] string userId){
            var feed=_postService.GetHomeFeedByUserId(userId);
            if(feed!=null){
                return Ok(feed);
            }
            return NotFound();
        }

        [HttpGet("userfeed/{userId}")]
        public ActionResult<IEnumerable<PublishedPostModel>> GetUserFeed(string userId){
            var feed=_postService.GetUserFeedByUserId(userId);
            if(feed!=null){
                return Ok(feed);
            }
            return NotFound();
        }
    }
}