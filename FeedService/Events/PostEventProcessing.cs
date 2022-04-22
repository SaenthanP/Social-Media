using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Events{
    public class PostEventProcessing : IPostEventProcessing
    {
        private readonly IDatabase _redis;
        private readonly INetworkEventProcessing _networkEventProcessing;

        public PostEventProcessing(IConnectionMultiplexer muxer,INetworkEventProcessing networkEventProcessing )
        {
            _redis=muxer.GetDatabase();
            _networkEventProcessing=networkEventProcessing;
        }
        public void AddPostToFeed(PublishedPostDto publishedPostDto)
        {
            _redis.ListRightPush(publishedPostDto.UserId+"Home Feed",publishedPostDto.PostId);
            
            var followerList=_networkEventProcessing.GetFollowListByUserId(publishedPostDto.UserId);

            foreach(var userId in followerList){
                _redis.ListRightPush((string)userId+"User Feed",publishedPostDto.PostId);
            }

            _redis.StringSet(publishedPostDto.PostId,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(publishedPostDto)));
        }

        public IEnumerable<PublishedPostDto> GetHomeFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+"Home Feed");
            if(postList!=null){
                IEnumerable<PublishedPostDto> homeFeed=postList.Select(u=>JsonSerializer.Deserialize<PublishedPostDto>(Encoding.UTF8.GetString(_redis.StringGet((string)u))));
                return homeFeed;
            }
            
            return null;
        }
    }
}