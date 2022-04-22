using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Services{
    public class NetworkService:INetworkService{
        private readonly IDatabase _redis;

        public NetworkService(IConnectionMultiplexer muxer)
        {
           _redis=muxer.GetDatabase();
        }
        public void AddFollowToCache(PublishedNetworkDto publishedNetworkDto)
        {
            _redis.ListRightPush(publishedNetworkDto.UserToFollowId+"Follower List",publishedNetworkDto.UserId);

        }

        public RedisValue[] GetFollowListByUserId(string userId)
        {
            var followerList2=_redis.ListRange(userId+"Follower List");

            return followerList2;
        }
    }
}