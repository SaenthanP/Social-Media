using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FeedService.Dtos;
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
        public void AddPostToFeed(PublishedPostDto publishedPostDto)
        {
            _redis.ListLeftPush(publishedPostDto.UserId+"User Feed",publishedPostDto.PostId);
            
            var followerList=_networkService.GetFollowerListByUserId(publishedPostDto.UserId);

            foreach(var userId in followerList){
                _redis.ListLeftPush(userId+"Home Feed",publishedPostDto.PostId);
            }

            _redis.StringSet(publishedPostDto.PostId,Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(publishedPostDto)));
        }
        private IEnumerable<PublishedPostDto> GetFeedFromRedisList(RedisValue[] postList){
            if(postList!=null){
                IEnumerable<PublishedPostDto> userFeed=Enumerable.Empty<PublishedPostDto>();
                foreach(var post in postList){
                    userFeed=userFeed.Append(JsonConvert.DeserializeObject<PublishedPostDto>(Encoding.UTF8.GetString(_redis.StringGet((string)post))));
                }
            
                return userFeed;
            }

            return null;
        }

        public IEnumerable<PublishedPostDto> GetHomeFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+"Home Feed");

            var homeFeed=GetFeedFromRedisList(postList);
            return homeFeed;
        }

        public IEnumerable<PublishedPostDto> GetUserFeedByUserId(string userId)
        {
            var postList=_redis.ListRange(userId+"User Feed");
            
            var userFeed=GetFeedFromRedisList(postList);
            return userFeed;
        }

        public void ReBuildHomeFeed(string userId)
        {
            /*
            Grabs all the posts for the following list, sort by the posted date, and replace the feed in the cache
            */
            IEnumerable<PublishedPostDto> newFeed=Enumerable.Empty<PublishedPostDto>();
            var followingList=_networkService.GetFollowingListByUserId(userId);

            foreach(var following in followingList){
                newFeed=newFeed.Concat(GetUserFeedByUserId(following));
            }

            newFeed=newFeed.OrderBy(p=>p.PostDateTime);
            _redis.KeyDelete(userId+"Home Feed");

            foreach(var post in newFeed){
                _redis.ListLeftPush(userId+"Home Feed",post.PostId);
            }
    
        }

    }
}