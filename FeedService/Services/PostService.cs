using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FeedService.Models;
using FeedService.EventConstants;
using Newtonsoft.Json;
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
        public void AddPostToFeed(PublishedPostModel publishedPostModel)
        {
            _redis.ListLeftPush(publishedPostModel.UserId+PostConstants.USER_FEED,publishedPostModel.PostId);
            
            var followerList=_networkService.GetFollowerListByUserId(publishedPostModel.UserId);

            foreach(var userId in followerList){
                _redis.ListLeftPush(userId+PostConstants.HOME_FEED,publishedPostModel.PostId);
            }

            _redis.StringSet(publishedPostModel.PostId,Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(publishedPostModel)));
        }
        private IEnumerable<PublishedPostModel> GetFeedFromRedisList(RedisValue[] postList){
            if(postList!=null){
                IEnumerable<PublishedPostModel> userFeed=Enumerable.Empty<PublishedPostModel>();
                foreach(var postId in postList){
                    userFeed=userFeed.Append(JsonConvert.DeserializeObject<PublishedPostModel>(Encoding.UTF8.GetString(_redis.StringGet((string)postId))));
                }
            
                return userFeed;
            }

            return null;
        }

        public IEnumerable<PublishedPostModel> GetHomeFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+PostConstants.HOME_FEED);

            var homeFeed=GetFeedFromRedisList(postList);
            return homeFeed;
        }

        public IEnumerable<PublishedPostModel> GetUserFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+PostConstants.USER_FEED);
            
            var userFeed=GetFeedFromRedisList(postList);
            return userFeed;
        }

        public void ReBuildHomeFeed(string userId)
        {
            /*
            Grabs all the posts for the following list, sort by the posted date, and replace the feed in the cache
            */
            IEnumerable<PublishedPostModel> newFeed=Enumerable.Empty<PublishedPostModel>();
            var followingList=_networkService.GetFollowingListByUserId(userId);

            foreach(var following in followingList){
                newFeed=newFeed.Concat(GetUserFeedByUserId(following));
            }

            newFeed=newFeed.OrderBy(p=>p.PostDateTime);
            _redis.KeyDelete(userId+PostConstants.HOME_FEED);

            foreach(var post in newFeed){
                _redis.ListLeftPush(userId+PostConstants.HOME_FEED,post.PostId);
            }
    
        }

    }
}