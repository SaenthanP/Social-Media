using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeedService.Models;
using FeedService.EventConstants;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FeedService.Services{
    public class NetworkService:INetworkService{
        private readonly IDatabase _redis;

        public NetworkService(IConnectionMultiplexer muxer)
        {
           _redis=muxer.GetDatabase();
        }
        public void AddFollowToCache(PublishedNetworkModel publishedNetworkModel)
        {
            _redis.ListRightPush(publishedNetworkModel.UserToInteractWithId+NetworkConstants.FOLLOWER_LIST,publishedNetworkModel.UserId);
            _redis.ListRightPush(publishedNetworkModel.UserId+NetworkConstants.FOLLOWING_LIST,publishedNetworkModel.UserToInteractWithId);
        }

        public IEnumerable<string> GetFollowingListByUserId(string userId)
        {
            var followingList=_redis.ListRange(userId+NetworkConstants.FOLLOWING_LIST).Select(u=>(string)u);
            return followingList;
        }

        public IEnumerable<string> GetFollowerListByUserId(string userId)
        {
            var followerList=_redis.ListRange(userId+NetworkConstants.FOLLOWER_LIST).Select(u=>(string)u);
            return followerList;
        }

        public void RemoveFollowFromCache(PublishedNetworkModel publishedNetworkModel)
        {
            _redis.ListRemove(publishedNetworkModel.UserToInteractWithId+NetworkConstants.FOLLOWER_LIST,publishedNetworkModel.UserId);
            _redis.ListRemove(publishedNetworkModel.UserId+NetworkConstants.FOLLOWING_LIST,publishedNetworkModel.UserToInteractWithId);
        }
    }
}