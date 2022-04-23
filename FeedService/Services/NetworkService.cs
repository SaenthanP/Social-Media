using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeedService.Dtos;
using Newtonsoft.Json;
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
            _redis.ListRightPush(publishedNetworkDto.UserId+"Following List",publishedNetworkDto.UserToFollowId);

        }

        public IEnumerable<string> GetFollowingListByUserId(string userId)
        {
            var followingList=_redis.ListRange(userId+"Following List").Select(u=>(string)u);
            return followingList;
        }

        IEnumerable<string> INetworkService.GetFollowerListByUserId(string userId)
        {
            var followerList=_redis.ListRange(userId+"Follower List").Select(u=>(string)u);
            return followerList;
        }
    }
}