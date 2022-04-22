using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Services{
    public class PostService : IPostService
    {
        private readonly IDatabase _redis;
        private readonly INetworkService _networkService;

        public PostService(IConnectionMultiplexer muxer,INetworkService networkService )
        {
            _redis=muxer.GetDatabase();
            _networkService=networkService;
        }
        public void AddPostToFeed(PublishedPostDto publishedPostDto)
        {
            _redis.ListRightPush(publishedPostDto.UserId+"User Feed",publishedPostDto.PostId);
            
            var followerList=_networkService.GetFollowListByUserId(publishedPostDto.UserId);

            foreach(var userId in followerList){
                _redis.ListRightPush((string)userId+"Home Feed",publishedPostDto.PostId);
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

        public IEnumerable<PublishedPostDto> GetUserFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+"User Feed");
            if(postList!=null){
                IEnumerable<PublishedPostDto> userFeed=postList.Select(u=>JsonSerializer.Deserialize<PublishedPostDto>(Encoding.UTF8.GetString(_redis.StringGet((string)u))));

                return userFeed;
            }

            return null;
        }
    }
}